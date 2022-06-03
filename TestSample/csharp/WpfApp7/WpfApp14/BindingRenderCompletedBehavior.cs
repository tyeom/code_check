namespace WpfApp14
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Threading;

    internal class BindingRenderCompletedBehavior : DependencyObject
    {
        public BindingRenderCompletedBehavior()
        {
            
        }

        public static ICommand GetRenderCompletedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(RenderCompletedCommandProperty);
        }

        public static void SetRenderCompletedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(RenderCompletedCommandProperty, value);
        }

        private static DependencyProperty RenderCompletedCommandProperty =
            DependencyProperty.RegisterAttached("RenderCompletedCommand",
                typeof(ICommand),
                typeof(BindingRenderCompletedBehavior),
                new UIPropertyMetadata(null, (d, e) =>
                {
                    FrameworkElement element = (FrameworkElement)d;

                    element.Dispatcher.BeginInvoke(
                        (System.Threading.ThreadStart)(() =>
                        {
                            ((ICommand)e.NewValue).Execute(null);
                        }), DispatcherPriority.ApplicationIdle);
                }));

        public static readonly DependencyProperty RenderCompletedCommandParameterProperty =
            DependencyProperty.RegisterAttached(
                "RenderCompletedCommandParameter",
                typeof(Object),
                typeof(BindingRenderCompletedBehavior),
                new UIPropertyMetadata(null));

        public Object? RenderCompletedCommandParameter
        {
            get
            {
                return (Object?)GetValue(RenderCompletedCommandParameterProperty);
            }
            set
            {
                SetValue(RenderCompletedCommandParameterProperty, value);
            }
        }
    }
}
