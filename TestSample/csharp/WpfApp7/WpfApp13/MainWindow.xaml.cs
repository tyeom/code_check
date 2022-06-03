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

namespace WpfApp13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        Button btn;

        public MainWindow()
        {
            InitializeComponent();
            //this.DataContext = new MainViewModel(this);
            this.DataContext = App.Current.Services.GetService(typeof(MainViewModel));

            btn = new Button();
            btn.Click += xBtnTest_Click;
            btn.Content = "코드 생성 테스트 버튼";
            btn.Width = 200;
            btn.Height = 200;
            var controlTemplate = btn.Template.LoadContent();

            btn.Template.RegisterName("border", btn);

            var dd = btn.Template.FindName("border", btn);
            if (controlTemplate is Border border)
            {
                border.CornerRadius = new CornerRadius(10);
            }
            //btn.OnApplyTemplate();
            
            //this.xGrid.Children.Add(btn);
        }

        public bool? ShowPopupWindow()
        {
            PopWindow popWindow = new();
            return popWindow.ShowDialog();
        }

        private void xBtnTest_Click(object sender, RoutedEventArgs e)
        {
            //var controlTemplate = this.xBtnTest.Template.LoadContent();
            //if (controlTemplate is Border border)
            //{
            //    //border.CornerRadius = new CornerRadius(10);
            //}

            ////controlTemplate.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
            ////btn.ApplyTemplate();
            ////btn.OnApplyTemplate();

            

            //var dd = this.xBtnTest.Template.FindName("border", this.xBtnTest);
            //if (dd is Border border2)
            //{
            //    border2.CornerRadius = new CornerRadius(100);
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
