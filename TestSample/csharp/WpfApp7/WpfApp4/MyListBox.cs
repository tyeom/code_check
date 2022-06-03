namespace WpfApp4
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class MyListBox : ListBox
    {
        public static readonly DependencyProperty ContextMenuTargetPoroperty =
          DependencyProperty.Register("ContextMenuTarget", typeof(TestModel), typeof(MyListBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public TestModel ContextMenuTarget
        {
            get { return base.GetValue(ContextMenuTargetPoroperty) as TestModel; }
            set { base.SetValue(ContextMenuTargetPoroperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyListItem();
        }
    }
}
