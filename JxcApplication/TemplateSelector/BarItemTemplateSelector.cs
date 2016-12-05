using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace JxcApplication.TemplateSelector
{
    public class BarItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate SubItemTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return ItemTemplate;
        }
    }
}
