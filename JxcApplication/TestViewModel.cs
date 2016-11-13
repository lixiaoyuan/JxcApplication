using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using JxcApplication.ViewModels;

namespace JxcApplication
{
    [POCOViewModel]
   public class TestViewModel : ViewModelBaseCor
    {
        public void Test()
        {
            DXMessageBox.Show("TestViewModel");
        }
    }

    [POCOViewModel]
    public class TestViewModel2 : ViewModelBaseCor
    {
        public void Test()
        {
            DXMessageBox.Show("TestViewModel2");
        }
    }
}
