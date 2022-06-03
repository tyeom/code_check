using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace FaceApiClient
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private FaceAPI.FaceAPI _faceAPI;
        public MainWindow()
        {
            InitializeComponent();

            _faceAPI = new FaceAPI.FaceAPI();
            _faceAPI.Init();
        }

        private void Local_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _faceAPI.GetFaceAttributesForLocal(openFileDialog.FileName, this.GetFaceAttributesCallback);

                this.xImg.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void Remote_Click(object sender, RoutedEventArgs e)
        {
            string imageUrl = "https://ssl.pstatic.net/mimgnews/image/079/2011/02/22/22155602009000_61000090.jpg";
            _faceAPI.GetFaceAttributesForUrl(imageUrl, this.GetFaceAttributesCallback);

            this.xImg.Source = new BitmapImage(new Uri(imageUrl));
        }

        private void GetFaceAttributesCallback(double? age, FaceAPI.FaceAPI.Gender? gender)
        {
            string result = string.Format("Age : {0} / Gender : {1}", age.ToString(), gender.ToString());
            MessageBox.Show(result);
        }
    }
}
