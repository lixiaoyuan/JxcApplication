using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ApplicationDb.Cor.EntityModels;
using JxcApplication.Core.WorkApprovalData;

namespace JxcApplication.Convert
{
    public class DynamicTemplateData<T> where T:class 
    {
        public string TemplateName { get; set; }
        public WorkApproval WorkApproval { get; set; }
        public T FromData { get; set; }
    }

    public class WorkApprovalToDataContextConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value  is WorkApproval)
            {
                var workApproval = value as WorkApproval;
                if (string.IsNullOrWhiteSpace(workApproval.DataType)||string.IsNullOrWhiteSpace(workApproval.FormDataTemplate))
                {
                    return null;
                }
                else
                {
                    var dataType = Type.GetType(workApproval.DataType);
                    if (dataType == null)
                    {
                        return null;
                    }
                    Type type = typeof(DynamicTemplateData<>);
                    type = type.MakeGenericType(dataType);
                    var returnObject = Activator.CreateInstance(type);

                    var newObject = Activator.CreateInstance(dataType);

                    var FromDataProp = returnObject.GetType().GetProperty("FromData");
                    if (FromDataProp != null)
                    {
                        FromDataProp.SetValue(returnObject, newObject, null);
                    }
                    var templateNameProp = returnObject.GetType().GetProperty("TemplateName");
                    if (templateNameProp != null)
                    {
                        templateNameProp.SetValue(returnObject, workApproval.FormDataTemplate, null);
                    }
                    var workApprovalProp = returnObject.GetType().GetProperty("WorkApproval");
                    if (workApprovalProp != null)
                    {
                        workApprovalProp.SetValue(returnObject, workApproval, null);
                    }
                    return returnObject;
                }
            }
            else
            {
                //throw new NotSupportedException("不支持此类型转换!");
                return null;
            }
          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
