using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxcApplication.Core
{
    public class SortCodeGenerate
    {
        public static string GetCode(string head, string lastCode)
        {
            if (string.IsNullOrWhiteSpace(lastCode))
            {
                return head + "1".PadLeft(4, '0');
            }
            int i = lastCode.IndexOf(head, StringComparison.CurrentCultureIgnoreCase);
            if (i < 0)
            {
                return head + "1".PadLeft(4, '0');
            }
            else
            {
                int outResult = 0;
                if (int.TryParse(lastCode.Substring(i + head.Length), out outResult))
                {
                    outResult += 1;
                    return head + outResult.ToString().PadLeft(4, '0');
                }
                else
                {
                    return head + "1".PadLeft(4, '0');
                }
            }

        }
    }
}
