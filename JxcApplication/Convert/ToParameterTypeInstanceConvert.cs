using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace JxcApplication.Convert
{
    public class ToParameterTypeInstanceConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            targetType = parameter as Type;
            if (targetType == null)
            {
                return null;
            }
            return Activator.CreateInstance(targetType, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
