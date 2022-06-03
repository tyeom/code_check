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

namespace SimpleMVVMWpf
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            System.Collections.Generic.List<int> a = new System.Collections.Generic.List<int>();
            Random rnd = new Random();
            int v = rnd.Next(0, 10);
            a.Add(v);

            System.Threading.Thread.Sleep(300);
            rnd = new Random();
            v = rnd.Next(0, 10);
            a.Add(v);

            System.Threading.Thread.Sleep(300);
            rnd = new Random();
            v = rnd.Next(0, 10);
            a.Add(v);

            System.Threading.Thread.Sleep(300);
            rnd = new Random();
            v = rnd.Next(0, 10);
            a.Add(v);

            System.Threading.Thread.Sleep(300);
            rnd = new Random();
            v = rnd.Next(0, 10);
            a.Add(v);

            System.Threading.Thread.Sleep(300);
            rnd = new Random();
            v = rnd.Next(0, 10);
            a.Add(v);

            InitializeComponent();
        }
    }
}
