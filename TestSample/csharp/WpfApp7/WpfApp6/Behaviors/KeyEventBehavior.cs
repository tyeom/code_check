namespace WpfApp6.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public class KeyEventBehavior : DependencyObject
    {
        public static readonly DependencyProperty IsKeyEventCommandProperty =
            DependencyProperty.RegisterAttached("IsKeyEventCommand", typeof(bool), typeof(KeyEventBehavior),
                new UIPropertyMetadata(false, null,
                    (o, v) =>
                    {
                        FrameworkElement element = o as FrameworkElement;
                        if (element == null)
                        {
                            throw new ArgumentException("어태치 대상 컨트롤 타입이 FrameworkElement이 아닙니다!");
                        }

                        if ((bool)v)
                        {
                            element.PreviewKeyDown += Element_KeyDown;
                            element.PreviewKeyUp += Element_KeyUp;
                        }
                        else
                        {
                            element.PreviewKeyDown -= Element_KeyDown;
                            element.PreviewKeyUp -= Element_KeyUp;
                        }

                        return v;
                    })
                );

        public static bool GetIsKeyEventCommand(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsKeyEventCommandProperty);
        }

        public static void SetIsKeyEventCommand(DependencyObject obj, bool value)
        {
            obj.SetValue(IsKeyEventCommandProperty, value);
        }

        public static readonly DependencyProperty KeyDownCommandProperty =
            DependencyProperty.RegisterAttached("KeyDownCommand", typeof(ICommand), typeof(KeyEventBehavior), new UIPropertyMetadata(null));

        public static ICommand GetKeyDownCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(KeyDownCommandProperty);
        }

        public static void SetKeyDownCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(KeyDownCommandProperty, value);
        }

        public static readonly DependencyProperty KeyUpCommandProperty =
            DependencyProperty.RegisterAttached("KeyUpCommand", typeof(ICommand), typeof(KeyEventBehavior), new UIPropertyMetadata(null));

        public static ICommand GetKeyUpCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(KeyUpCommandProperty);
        }

        public static void SetKeyUpCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(KeyUpCommandProperty, value);
        }

        private static void Element_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is DependencyObject obj)
            {
                ICommand command = GetKeyUpCommand(obj);
                if (command != null)
                {
                    command.Execute(e.Key);
                }
            }
        }

        private static void Element_KeyDown(object sender, KeyEventArgs e)
        {
            if(sender is DependencyObject obj)
            {
                ICommand command = GetKeyDownCommand(obj);
                if (command != null)
                {
                    command.Execute(e.Key);
                }
            }
        }
    }
}
