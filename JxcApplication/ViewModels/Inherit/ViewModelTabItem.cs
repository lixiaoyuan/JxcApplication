using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DevExpress.Mvvm.POCO;

namespace JxcApplication.ViewModels.Inherit
{
    public class ViewModelTabItem : ViewModelBaseCor
    {
        public bool Selected { get; set; }
        public string Caption { get; set; }
        public ViewModelTabItem(Guid menuId,string caption)
        {
            InitRibbonUi(menuId);
            Caption = caption;
        }
    }
}
