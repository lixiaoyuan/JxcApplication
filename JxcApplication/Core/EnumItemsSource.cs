using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using BusinessDb.Cor.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using Utilities;

namespace JxcApplication.Core
{

    public class EnumItemsSource : MarkupExtension
    {
        public Type EnumType { get; set; }

        public bool UseNumericEnumValue { get; set; }

        public bool SplitNames { get; set; }

        public IValueConverter NameConverter { get; set; }

        public EnumMembersSortMode SortMode { get; set; }

        public bool AllowImages { get; set; }

        public EnumItemsSource()
        {
            this.SplitNames = true;
            this.AllowImages = true;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (EnumType == (Type)null || !EnumType.IsEnum)
                return Enumerable.Empty<EnumMemberInfo>();


            var t = EnumType.GetFields(BindingFlags.Static | BindingFlags.Public)
                  .Where(a => DataAnnotationsAttributeHelper.GetAutoGenerateField(a))
                  .Select(a =>
                  {

                      var firstOrDefault = GetAttributes<DisplayAttribute>(a).FirstOrDefault();
                      if (firstOrDefault != null)
                      {
                          string displayAttribute = firstOrDefault.Description;
                          return new EnumMemberInfo(a.Name, displayAttribute, a.Name, null);
                      }
                      return new EnumMemberInfo(a.Name, "", CustomerType.Retail, null);
                  });
            return t;
        }

        public static bool IsEnumItemsSource(object itemsSource)
        {
            return itemsSource is IEnumerable<EnumMemberInfo>;
        }

        public static void SetupEnumItemsSource(object itemsSource, Action setupCallback)
        {
            if (!DevExpress.Xpf.Editors.EnumItemsSource.IsEnumItemsSource(itemsSource))
                return;
            setupCallback();
        }

        internal static T GetAttribute<T>(MemberInfo member, bool inherit = false) where T : Attribute
        {
            return Enumerable.FirstOrDefault<T>(GetAttributes<T>(member, inherit));
        }
        internal static IEnumerable<T> GetAttributes<T>(MemberInfo member, bool inherit = false) where T : Attribute
        {
            return Enumerable.OfType<T>((IEnumerable)GetAllAttributes(member, inherit));
        }
        internal static Attribute[] GetAllAttributes(MemberInfo member, bool inherit = false)
        {
            IEnumerable<Attribute> second = MetadataHelper.GetExternalAndFluentAPIAttrbutes(member.ReflectedType, member.Name) ?? (IEnumerable<Attribute>)new Attribute[0];
            return Enumerable.ToArray<Attribute>(Enumerable.Concat<Attribute>((IEnumerable<Attribute>)Attribute.GetCustomAttributes(member, inherit), second));
        }
    }
}
