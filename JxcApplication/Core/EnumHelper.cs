using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Native.ViewGenerator;
using DevExpress.Utils.Design;

namespace JxcApplication.Core
{
    public static class EnumHelper
    {
        public static int GetEnumCount(Type enumType)
        {
            return EnumSourceHelperCore.GetEnumCount(enumType);
        }

        public static IEnumerable<DevExpress.Mvvm.EnumMemberInfo> GetEnumSource(Type enumType, bool useUnderlyingEnumValue = true, IValueConverter nameConverter = null, bool splitNames = false, EnumMembersSortMode sortMode = EnumMembersSortMode.Default, bool allowImages = true, bool allowText = true)
        {
            return EnumSourceHelperCore.GetEnumSource(enumType, useUnderlyingEnumValue, nameConverter, splitNames, sortMode, ViewModelMetadataSource.GetKnownImageCallback(ImageType.Colored), allowImages, allowText);
        }
    }
}
