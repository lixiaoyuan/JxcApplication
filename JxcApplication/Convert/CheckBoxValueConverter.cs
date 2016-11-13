﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace JxcApplication.Convert
{
    public class CheckBoxValueConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }
            return value;
        }
    }
}
