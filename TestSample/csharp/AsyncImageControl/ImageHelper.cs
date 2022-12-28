using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media.Imaging;

namespace AsyncImageControl;

public class ImageHelper
{
    public static BitmapImage? CreateBitmapImage(string? imgFullPath, int decodePixelWidth = 300)
    {
        if (string.IsNullOrWhiteSpace(imgFullPath)) return null;

        string imageFileName = Path.GetFileName(imgFullPath!);
        if (File.Exists($"{PathHelper.GetLocalDownloadDirectory()}{imageFileName}") == false)
            return null;

        BitmapImage bitmapImg = new BitmapImage();
        bitmapImg.BeginInit();
        bitmapImg.CacheOption = BitmapCacheOption.OnDemand;
        bitmapImg.CreateOptions = BitmapCreateOptions.DelayCreation;
        bitmapImg.DecodePixelWidth = decodePixelWidth;
        bitmapImg.UriSource = new Uri($"{PathHelper.GetLocalDownloadDirectory()}{imageFileName}");
        bitmapImg.EndInit();

        return bitmapImg;
    }

    /// <summary>
    /// 비동기로 Network 이미지를 다운로드하는 BitmapImage 생성
    /// </summary>
    /// <param name="imgFullPath">이미지 경로 (url)</param>
    /// <param name="isLocalDownloadFile">로컬 이미지 여부</param>
    /// <param name="decodePixelWidth">width 해상도</param>
    /// <returns></returns>
    public static async Task<BitmapImage?> CreateBitmapImage(string? imgFullPath, bool isLocalDownloadFile, int timeout, bool isFileCache = false, int decodePixelWidth = 300)
    {
        if(string.IsNullOrWhiteSpace(imgFullPath)) return null;

        BitmapImage? bitmapImg = null;
        if (isLocalDownloadFile)
        {
            bitmapImg = CreateBitmapImage(imgFullPath!, decodePixelWidth);
        }

        if (bitmapImg != null)
            return bitmapImg;

        // 로컬에 이미지가 없다면 이미지 다운로드
        using (HttpClient client = new HttpClient(new HttpClientHandler { MaxConnectionsPerServer = 10 }))
        {
            // 타임아웃 CancellationToken
            var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
            var token = tokenSource.Token;

            HttpResponseMessage response = await client.GetAsync(imgFullPath, token);
            byte[] bytes = await response.Content.ReadAsByteArrayAsync();

            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImg.DecodePixelWidth = decodePixelWidth;
            bitmapImg.StreamSource = new MemoryStream(bytes);
            bitmapImg.EndInit();

            if (isFileCache)
            {
                string imageFileName = Path.GetFileName(imgFullPath!);
                string savePath = $"{PathHelper.GetLocalDownloadDirectory()}{imageFileName}";
                File.WriteAllBytes(savePath, bytes);
            }
        }

        return bitmapImg;
    }
}