using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp12
{
    public class ProgressTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WaitingTemplate
        { get; set; }

        public DataTemplate ProgressTemplate
        { get; set; }

        public DataTemplate CompleteTemplate
        { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            File file = item as File;

            if (file != null)
            {
                if (file.StatusFilesize == 0)
                {
                    return WaitingTemplate;
                }
                if (file.StatusFilesize == 100)
                {
                    return CompleteTemplate;
                }
                return ProgressTemplate;
            }
            else
                return base.SelectTemplate(item, container);
        }
    }
}
