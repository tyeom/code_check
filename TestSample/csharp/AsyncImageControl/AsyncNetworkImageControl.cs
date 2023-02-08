using System;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace AsyncImageControl;

public class AsyncNetworkImageControl : Image
{
    public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(String), typeof(AsyncNetworkImageControl),
                new PropertyMetadata(new PropertyChangedCallback(OnUrlChanged)));

    public string Url
    {
        get { return (string)GetValue(UrlProperty); }
        set { SetValue(UrlProperty, value); }
    }

    public int Timeout
    {
        get; set;
    } = 15;

    public bool IsLocalImg
    {
        get; set;
    } = true;

    public bool IsFileCache
    {
        get; set;
    } = false;

    private static async void OnUrlChanged(DependencyObject property, DependencyPropertyChangedEventArgs e)
    {
        AsyncNetworkImageControl? asyncNetworkImageControl = property as AsyncNetworkImageControl;
        if (asyncNetworkImageControl == null) return;

        try
        {
            BitmapImage? bitmapImage = await ImageHelper.CreateBitmapImage(e.NewValue?.ToString(),
                asyncNetworkImageControl.IsLocalImg,
                asyncNetworkImageControl.Timeout,
                asyncNetworkImageControl.IsFileCache);
            asyncNetworkImageControl.Source = bitmapImage;
            bitmapImage?.Freeze();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}