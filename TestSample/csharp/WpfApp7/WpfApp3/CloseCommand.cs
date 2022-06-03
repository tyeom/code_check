namespace WpfApp3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public class CloseCommand : DependencyObject
    {
        public static readonly DependencyProperty IsCloseProperty =
               DependencyProperty.RegisterAttached("IsClose", typeof(bool),
               typeof(CloseCommand), new FrameworkPropertyMetadata(OnIsClosePropertyChanged));

        public static void SetIsClose(DependencyObject obj, bool value)
        {
            obj.SetValue(IsCloseProperty, value);
        }

        public static bool GetIsClose(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsCloseProperty);
        }

        private static void OnIsClosePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Window window = Window.GetWindow(sender);
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    window.Close();
                }
            }
        }
    }
}
