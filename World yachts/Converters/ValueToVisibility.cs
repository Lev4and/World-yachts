using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace World_yachts.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class ValueToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible ? true : false;
        }
    }
}
