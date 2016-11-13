using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ApplicationDb.Cor.Model;
using JxcApplication.Model;

namespace JxcApplication.Selector
{
    public class CategoryTemplateSelector:DataTemplateSelector
    {
        public DataTemplate CategoryTemplate { get; set; }
        public DataTemplate DefaultCategoryTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            try
            {
                var model = item as RibbonNodeModel;
                if (model==null)
                {
                    return DefaultCategoryTemplate;
                }
                return model.IsDefaultPageCategory ? DefaultCategoryTemplate : CategoryTemplate;
            }
            catch (Exception)
            {
                return DefaultCategoryTemplate;
            }
        }
    }
}
