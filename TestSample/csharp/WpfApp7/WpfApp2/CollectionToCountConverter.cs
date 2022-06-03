namespace WpfApp2
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Data;

    public class CollectionToCountConverter : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            ICollection collection = value as ICollection;
            if (collection == null)
                return 0;
            return collection.Count;
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
