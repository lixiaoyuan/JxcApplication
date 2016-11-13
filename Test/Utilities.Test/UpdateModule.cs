using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utilities.Test
{
    [TestClass]
    public class UpdateModule
    {
        [TestMethod]
        public void CheckUpdate()
        {
            using (var down = new SharedDirectoryDownload())
            {
                var result = down.Connect("\\\\192.168.33.200\\Update", "dir", "123");
                if (!result)
                {
                    Assert.Fail("连接失败"); return;
                }
                global::Utilities.UpdateModule k = new global::Utilities.UpdateModule(down,"\\Update", "F:\\Update");
                result = k.CheckUpdate();
                if (result)
                {
                    Console.WriteLine("需要升级!");
                }
                else
                {
                    Console.WriteLine("不需要升级!");
                }
                result = k.Update();
                Console.WriteLine(result);
            }

        }

        [TestMethod]
        public void Test()
        {
            //var find = "201607262050";
            //var last = "201607262051";
            //Assert.IsFalse(string.IsNullOrWhiteSpace(find));
            //Assert.IsFalse(string.IsNullOrWhiteSpace(last));
            //var file=new FileInfo(@"F:\Update\data.ud");
            //Console.WriteLine(file.Name);
            //Console.WriteLine(this.GetType().Assembly.Location);
            //Process.Start(@"E:\外接系统\厦门比格维尔\ErpApplication\Launcher\bin\Debug\Launcher.exe", Path.GetFileName(this.GetType().Assembly.Location) + "5 6 3");
            //FileInfo fileInfo = new FileInfo(this.GetType().Assembly.Location);
            //Console.WriteLine(fileInfo.Directory);

            File.Move(@"F:\Update\data.ud.fud", @"F:\Update\data.ud");
        }
    }
}
