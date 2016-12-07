using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace JxcApplication.TemplateSelector
{
    public class NotNullTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item==null)
            {
                return null;
            }
            else
            {
                return DataTemplate;
            }
        }
    }
}
