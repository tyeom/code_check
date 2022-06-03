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

namespace WpfApp7
{
    /// <summary>
    /// LayoutControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LayoutControl : UserControl
    {
        public LayoutControl()
        {
            InitializeComponent();
        }

        //public static readonly DependencyProperty HeaderContentProperty =
        //    DependencyProperty.Register("HeaderContent", typeof(Object), typeof(LayoutControl));
        //public Object HeaderContent
        //{
        //    get { return (Object)GetValue(HeaderContentProperty); }
        //    set { SetValue(HeaderContentProperty, value); }
        //}

        //public static readonly DependencyProperty InnerContentProperty =
        //    DependencyProperty.Register("InnerContent", typeof(Object), typeof(LayoutControl));
        //public Object InnerContent
        //{
        //    get { return (Object)GetValue(InnerContentProperty); }
        //    set { SetValue(InnerContentProperty, value); }
        //}

        //public static readonly DependencyProperty FooterContentProperty =
        //    DependencyProperty.Register("FooterContent", typeof(Object), typeof(LayoutControl));
        //public Object FooterContent
        //{
        //    get { return (Object)GetValue(FooterContentProperty); }
        //    set { SetValue(FooterContentProperty, value); }
        //}
    }
}
