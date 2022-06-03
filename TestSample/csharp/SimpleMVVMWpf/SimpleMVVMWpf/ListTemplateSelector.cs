namespace SimpleMVVMWpf
{
    using SimpleMVVMWpf.Models;
    using System.Windows;
    using System.Windows.Controls;

    public class ListTemplateSelector : DataTemplateSelector
    {
        public ListTemplateSelector() {}

        public DataTemplate NomalTemplate { get; set; }
        public DataTemplate StackTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            SampleModel sampleModel = item as SampleModel;
            if (sampleModel == null)
                return NomalTemplate;

            if (sampleModel.IsStack)
            {
                return StackTemplate;
            }
            else
            {
                return NomalTemplate;
            }
        }
    }
}
