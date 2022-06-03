namespace WpfApp6.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public class CloseCommand11 : DependencyObject
    {
        public static readonly DependencyProperty IsAcceptButtonProperty =
               DependencyProperty.RegisterAttached("IsClose", typeof(bool),
               typeof(CloseCommand11), new FrameworkPropertyMetadata(OnIsClosePropertyChanged));

        private static ICommand _closeCommand;
        public static readonly DependencyProperty CloseCommandProperty =
               DependencyProperty.RegisterAttached("AcceptButtonCommand", typeof(ICommand),
               typeof(CloseCommand11),
               new FrameworkPropertyMetadata(OnCloseCommandPropertyChanged));

        private static void OnIsClosePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Window window = Window.GetWindow(sender);
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    SetWindow(window);
                }
            }
        }

        private static void OnCloseCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            _closeCommand = e.NewValue as ICommand;
        }

        private static void SetWindow(Window window)
        {
            _closeCommand = new RelayCommand<Object>(new Action<Object>((param) => window.Close()));
        }
    }
}
