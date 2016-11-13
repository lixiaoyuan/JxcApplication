using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Markup;

namespace JxcApplication.Core
{
    public class EnumItemsSource2: MarkupExtension
    {
        public Type EnumType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetNames(EnumType).Select(a =>
            {
                var field = EnumType.GetField(a);
                var attribute = field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                if (attribute == null)
                {
                    return null;
                }
                return new
                {
                    Id = Enum.Parse(EnumType, a),
                    Description = ((DisplayAttribute)attribute).Description,
                    Value=(int)Enum.Parse(EnumType,a)
                };
            }).Where(a => a != null);
        }
    }
}
