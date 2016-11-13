using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintDocument printDocument=new PrintDocument();
            Console.WriteLine(printDocument.DefaultPageSettings.PaperSize);
            Console.WriteLine(printDocument.DefaultPageSettings.Margins);
            Console.WriteLine(printDocument.DefaultPageSettings.ToString());
            Console.ReadKey();}
    }
}
