using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace JxcApplication.Core
{

    public class EnumMemberInfo
    {
        public string Name { get; private set; }

        public bool ShowName { get; private set; }

        public object Id { get; private set; }

        public string Description { get;  set; }

        public ImageSource Image { get; private set; }

        public bool ShowImage { get; private set; }

        public int? Order { get; private set; }

        public EnumMemberInfo(string value, string description, object id, ImageSource image)
          : this(value, description, id, image, true, true, new int?())
        {
            this.Name = value;
            this.Description = description;
            this.Id = id;
            this.Image = image;
        }

        public EnumMemberInfo(string value, string description, object id, ImageSource image, bool showImage, bool showName, int? order = null)
        {
            this.Name = value;
            this.Description = description;
            this.Id = id;
            this.Image = image;
            this.ShowImage = showImage;
            this.ShowName = showName;
            this.Order = order;
        }

        //public override string ToString()
        //{
        //    return this.Name.ToString();
        //}

        //public override bool Equals(object obj)
        //{
        //    return object.Equals(this.Id, MayBe.Return<DevExpress.Mvvm.EnumMemberInfo, object>(obj as DevExpress.Mvvm.EnumMemberInfo, (Func<DevExpress.Mvvm.EnumMemberInfo, object>)(o => o.Id), (Func<object>)(() => (object)null)));
        //}

        //public override int GetHashCode()
        //{
        //    return this.Id.GetHashCode();
        //}
    }
}
