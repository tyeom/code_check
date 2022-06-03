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
using System.Windows.Shapes;

namespace WpfApp10
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window
    {
        private Window? _hostWindow;
        private UserControl? _uc;

        public Window1(UserControl uc)
        {
            InitializeComponent();

            _uc = uc;
            this.xUC.Loaded += this.uc_Loaded;
        }

        private void uc_Loaded(object sender, RoutedEventArgs e)
        {
            _hostWindow = GetWindow(_uc);
            this.Owner = _hostWindow;
            this.ShowDialog();
        }
    }
}
