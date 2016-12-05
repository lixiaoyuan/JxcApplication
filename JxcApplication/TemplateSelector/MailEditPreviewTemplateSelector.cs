using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ApplicationDb.Cor.Model;
using DevExpress.Utils;
using DevExpress.Xpf.Core.Native;
using JxcApplication.ViewModels.Mail;

namespace JxcApplication.TemplateSelector
{
    [ContentProperty("Templates")]
    public class MailEditPreviewTemplateSelector: DataTemplateSelector
    {
        public TemplatesDictionary Templates { get; set; }

        public MailEditPreviewTemplateSelector()
        {
            this.Templates = new TemplatesDictionary();
        }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //return base.SelectTemplate(item, container);
            var edit = item as IMailEditPreview;
            if (edit == null || edit.PreviewType == MailEditPreviewType.Null) return null;

            var field = typeof(MailEditPreviewType).GetField(edit.PreviewType.ToString());
            var attribute = field.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            if (attribute == null || !(attribute is DisplayAttribute))
            {
                return null;
            }
            var templateName = ((DisplayAttribute) attribute).Description;
            return Templates[templateName];
        }
    }
}
