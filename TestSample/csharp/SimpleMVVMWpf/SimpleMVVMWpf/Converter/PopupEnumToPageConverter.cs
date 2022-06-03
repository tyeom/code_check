namespace SimpleMVVMWpf.Converter
{
    using SimpleMVVMWpf.Common;
    using System;
    using System.Windows.Data;

    public class PopupEnumToPageConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is Enum && ((int)value) > 0)
            {
                string page = ((Enum)value).GetEnumAttributeValue<string>(0);
                Uri uri = new Uri(page, UriKind.Relative);
                return uri.ToString();
            }

            return null;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
