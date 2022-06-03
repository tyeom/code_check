namespace WpfApp4
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class MyListItem : ListBoxItem
    {
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element)
            {
                var parent = FindParent<MyListBox>(this);

                if (element.DataContext is TestModel model)
                {
                    if (parent != null)
                        parent.ContextMenuTarget = model;

                    //MessageBox.Show($"우 클릭으로 선택된 GroupName은 {model.GroupName} 입니다!");
                }
            }
        }

        private static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null) return null;
            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }
    }
}
