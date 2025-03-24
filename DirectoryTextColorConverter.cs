using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace D64MauiApp.Converters
{
    public class DirectoryTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isDirectory && isDirectory)
            {
                return Application.Current.Resources["C64Yellow"] as Color;
            }
            return Application.Current.Resources["C64White"] as Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
