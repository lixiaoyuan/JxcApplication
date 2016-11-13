using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using BusinessDb.Cor.Models;
using DevExpress.Mvvm;

namespace JxcApplication.Convert
{
    //public class RefundTypeEnumDisplayConvert : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        try
    //        {
    //            if (value == null)
    //            {
    //                return "";
    //            }
    //            RefundType rt = (RefundType)Enum.Parse(typeof(RefundType), value.ToString());
    //            switch (rt)
    //            {
    //                case RefundType.OffsetTransactions:
    //                    return "冲抵往来";
    //                case RefundType.RefundCash:
    //                    return "退还现金";
    //                case RefundType.Rebate:
    //                    return "返  利";
    //            }
    //            return "";
    //        }
    //        catch (Exception)
    //        {
    //            return "";
    //        }
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        try
    //        {
    //            RefundType rt = (RefundType)value;
    //            switch (rt)
    //            {
    //                case RefundType.OffsetTransactions:
    //                    return "OffsetTransactions";
    //                case RefundType.RefundCash:
    //                    return "RefundCash";
    //                case RefundType.Rebate:
    //                    return "Rebate";
    //            }
    //            return null;
    //        }
    //        catch (Exception)
    //        {
    //            return null;
    //        }
    //    }
    //}

    public class RefundTypeCast : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null)
            {
                return false;
            }
            RefundType rt = (RefundType)Enum.Parse(typeof(RefundType),value.ToString());
            return rt == RefundType.RefundCash;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RefundTypeRebate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            RefundType rt = (RefundType)Enum.Parse(typeof(RefundType), value.ToString());
            return rt != RefundType.Rebate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class OrderTypeEnumDisplayConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return "";
                }
                OrderType rt = (OrderType)Enum.Parse(typeof(OrderType), value.ToString());
                //取Display->Description
                
                switch (rt)
                {
                    case OrderType.XT:
                        return "销售退单";
                    case OrderType.XK:
                        return "销售开单";
                        case OrderType.QL:
                        return "清零单";
                }
                return "";
            }
            catch (Exception )
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                OrderType rt = (OrderType)value;
                switch (rt)
                {
                    case OrderType.XK:
                        return "XK";
                    case OrderType.XT:
                        return "XT";
                }
                return null;
            }
            catch (Exception )
            {
                return null;
            }
        }
    }

}
