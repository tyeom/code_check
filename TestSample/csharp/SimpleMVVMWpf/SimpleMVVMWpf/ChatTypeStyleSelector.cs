namespace SimpleMVVMWpf
{
    using SimpleMVVMWpf.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public class ChatTypeStyleSelector : DataTemplateSelector
    {
        public ChatTypeStyleSelector() {}

        public DataTemplate NomalTemplate { get; set; }
        public DataTemplate PhotoTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ChatModel chatModel = item as ChatModel;
            if (chatModel == null)
                return NomalTemplate;

            if (chatModel.ChatType == ChatModel.EChatType.Normal)
            {
                return NomalTemplate;
            }
            else if (chatModel.ChatType == ChatModel.EChatType.Photo)
            {
                return PhotoTemplate;
            }
            else
            {
                return NomalTemplate;
            }
        }
    }
}
