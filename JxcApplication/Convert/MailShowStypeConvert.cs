using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using JxcApplication.Core.Mail;

namespace JxcApplication.Convert
{
    public class MailShowStypeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var showType = value as MailListType?;
            if (showType == null)
            {
                return false;
            }
            var targetShowType = parameter as MailListType?;
            if (targetShowType == null)
            {
                return false;
            }
            return showType == targetShowType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
