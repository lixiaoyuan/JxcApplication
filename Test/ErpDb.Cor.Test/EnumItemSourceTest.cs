using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BusinessDb.Cor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErpDb.Cor.Test
{
    [TestClass]
    public class EnumItemSourceTest
    {
        [TestMethod]
        public void TestMainSource()
        {
            Type EnumType = typeof (CustomerType);
            var list = Enum.GetNames(EnumType);
           
            var results = list.Select(a =>
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
                     Description = ((DisplayAttribute)attribute).Description
                 };
             });
            foreach (var result in results)
            {
                Console.WriteLine(result.Id);
                Console.WriteLine(result.Description);
            }

        }
    }
}
