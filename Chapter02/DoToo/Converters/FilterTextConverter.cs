namespace DoToo.Converters;

using System;
using System.Globalization;

internal class FilterTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (bool)value ? "All" : "Active";

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
}