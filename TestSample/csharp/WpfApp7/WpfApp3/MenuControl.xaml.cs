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

namespace WpfApp3
{
    /// <summary>
    /// MenuControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
            this.DataContext = this;

            this.Loaded += MenuControl_Loaded;
        }

        private void MenuControl_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public static readonly DependencyProperty MenuInfoPoroperty =
          DependencyProperty.Register("MenuInfo", typeof(List<MenuModel>), typeof(MenuControl), new PropertyMetadata(null));
        public List<MenuModel> MenuInfo
        {
            get { return base.GetValue(MenuInfoPoroperty) as List<MenuModel>; }
            set { base.SetValue(MenuInfoPoroperty, value); }
        }

        private void Button_Click()
        {

        }
    }
}
