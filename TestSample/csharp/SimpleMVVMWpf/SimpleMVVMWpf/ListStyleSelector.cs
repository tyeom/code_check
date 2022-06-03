namespace SimpleMVVMWpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class ListStyleSelector : StyleSelector
    {
        public Style NomalStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            return NomalStyle;
        }
    }
}
