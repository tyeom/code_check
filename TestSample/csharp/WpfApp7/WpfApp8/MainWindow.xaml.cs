namespace WpfApp8
{
    using Microsoft.Win32;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
        }

        private async void xFileOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDig = new OpenFileDialog();
            if(openFileDig.ShowDialog() == true)
            {
                this.xSpinner.Visibility = Visibility.Visible;

                string filePath = openFileDig.FileName;

                this.xStateTxt.Text = "텍스트 파일 분석중..";
                // 파일을 List 배열로 변환
                List<string> fileContentByLine = await this.ConvertToListAsync(filePath);

                this.xStateTxt.Text = "번역중..";
                // List 배열로 변환된 것을 순수 한글만 추출해서 번역기 API 실행
                await this.TranslateForStringList(fileContentByLine);

                this.xSpinner.Visibility = Visibility.Collapsed;

                string saveFilePath = System.IO.Path.GetDirectoryName(filePath) + "\\" + "teanslate.txt";
                if (File.Exists(saveFilePath))
                    File.Delete(saveFilePath);

                File.AppendAllLines(saveFilePath, fileContentByLine);
            }
        }

        private async Task<List<string>> ConvertToListAsync(string filePath)
        {
            var task = Task<List<string>>.Run( () => ConvertToList(filePath) );

            return await task;
        }

        private List<string> ConvertToList(string filePath)
        {
            string[] fileContentByLine = File.ReadAllLines(filePath);
            if (fileContentByLine == null)
                return null;

            List<string> textContent = new List<string>();
            foreach (string line in fileContentByLine)
            {
                textContent.Add(line);
            }

            return textContent;
        }

        private async Task TranslateForStringList(List<string> strList)
        {
            if (strList == null) return;

            TranslateAPI api = new TranslateAPI();

            try
            {
                for (int i = 0; i < strList.Count; i++)
                {
                    int startPos = strList[i].LastIndexOf(": \"");
                    int endPos = strList[i].LastIndexOf("\"");
                    if (startPos < 0 || endPos < 0) continue;
                    
                    int tmpEndPos = endPos - 3 - startPos;
                    if (tmpEndPos < 0)
                    {
                        this.xStateTxt.Text = $"{strList[i]} 라인 번역 실패!!";
                        continue;
                    }
                    // 한글 문자 번역 대상 추출
                    string targetStr_KO = strList[i].Substring(startPos + 3, tmpEndPos);

                    // 번역 결과
                    string translateResult = await api.APIRequest($"source=ko&target=en&text={targetStr_KO}");
                    if (translateResult == null)
                    {
                        this.xStateTxt.Text = $"번역중 : {targetStr_KO} => 번역 요청 실패!!";
                        continue;
                    }

                    var placeInfoObject = JObject.Parse(translateResult);
                    if (placeInfoObject["message"] == null)
                    {
                        this.xStateTxt.Text = $"번역중 : {targetStr_KO} => {placeInfoObject["errorMessage"].ToString()}";
                        continue;
                    }

                    string translateStr = placeInfoObject["message"]["result"]["translatedText"].ToString();

                    strList[i] = strList[i].Replace(targetStr_KO, translateStr);

                    this.xStateTxt.Text = $"번역중 : {targetStr_KO} => {translateStr}";
                }
            }  catch(Exception ex)
            {
                //
            }

            this.xStateTxt.Text = "번역이 완료 되었습니다!";
        }
    }

    public class MainViewModel
    {
        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 400;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 400;
    }
}
