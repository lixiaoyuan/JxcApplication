using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;

namespace JxcApplication.Core
{
    [ContentProperty("Templates")]
    public class FieldTemplateSelector : DataTemplateSelector
    {
        public TemplatesDictionary Templates { get; set; }

        public string FieldKeyName { get; set; }
        public FieldTemplateSelector()
        {
            this.Templates = new TemplatesDictionary();
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Debug.Write("SelectTemplate");
            string templateName = "DataTemplateDefault";
            DataTemplate dataTemplate1 = null;
            try
            {
                if (item == null)
                {
                    return null;
                }
                Type type = item.GetType();
                var propertyInfo = type.GetProperty(FieldKeyName);
                if (propertyInfo == null)
                {
                    return null;
                }
                var value = propertyInfo.GetValue(item, null);
                if (value == null)
                {
                    return null;
                }
                if (string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return null;
                }
                else
                {
                    templateName = value.ToString();
                }
            }
            finally
            {
                this.Templates.TryGetValue(templateName, out dataTemplate1);
            }
            return dataTemplate1;
        }
    }
}
