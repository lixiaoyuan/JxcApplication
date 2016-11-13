using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace JxcApplication.Convert
{
    public class ColorToStringConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null)
            {
                return null;
            }
            var convertFromString = ColorConverter.ConvertFromString(value.ToString());
            if (convertFromString != null)
                return (Color) convertFromString;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null)
            {
                return null;
            }
            return value.ToString();
        }
    }
}
