using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utilities.Test
{
    [TestClass]
    public class TestSubstring
    {
        [TestMethod]
        public void Substring()
        {
            string maxCode = "LXy5585";
            string defChar = "LXY";
            string result = "";
            int i = maxCode.IndexOf(defChar, StringComparison.CurrentCultureIgnoreCase);
            if (i < 0)
            {
                result = defChar + "1".PadLeft(4, '0');
            }
            else
            {
                int outResult = 0;
                if (int.TryParse(maxCode.Substring(i + defChar.Length), out outResult))
                {
                    outResult += 1;
                    result = defChar + outResult.ToString().PadLeft(4, '0');
                }
            }

          
            Console.WriteLine(result);
            //maxCode.Substring(maxCode.Length - 3, 2);
        }

        [TestMethod]
        public void TestPassword()
        {
            string passwor = "123456";
            string sl = "bd615834-5d9f-4501-bdf7-4d2001661d92";
            var t = PasswordHelper.Encrypt(ref passwor, ref sl);
            Console.WriteLine(passwor);
        }
    }
}
