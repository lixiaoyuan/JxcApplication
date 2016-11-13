using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace JxcApplication.Convert
{
    public class NumberToVisibilityConverter: IValueConverter
    {
        /// <summary>
        /// 边界
        /// </summary>
        public int Boundary { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = 0;
            int.TryParse(value.ToString(), out intValue);
            if (intValue >= Boundary)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
