using System;
using System.Linq.Expressions;
using JxcApplication.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JxcApplication.Test
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public void TestContor()
        {
            var type = typeof (RibbonNodeMaintainViewModel);
            var constructor = type.GetConstructor(new[] {typeof (Guid), typeof (string)});
            Expression<Func<Guid, string, RibbonNodeMaintainViewModel>> expre =
                (Guid _menuid, string _tabCaption) =>
                    new RibbonNodeMaintainViewModel(_menuid,_tabCaption);
            var t = expre.Body as NewExpression;

        }
    }
}