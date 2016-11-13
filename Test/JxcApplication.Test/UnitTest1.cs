using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using BusinessDb.Cor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DevExpress.Xpf.Editors;
using DevExpress.Mvvm;
using Utilities.Net;

namespace JxcApplication.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            EnumItemsSource source=new EnumItemsSource();
            source.EnumType = typeof (OrderType);
            EnumMemberInfo[] a = (EnumMemberInfo[])source.ProvideValue(null);
            foreach (EnumMemberInfo info in a)
            {
                Console.WriteLine(info.Description);
            }
        }

        [TestMethod]
        public void StringTest()
        {
            List<string> list=new List<string>();
            list.Add("201607262050");
            list.Add("201607262049");
            list.Add("201607262053");
            var ilist = list.OrderBy(s => s);
            foreach (string s in ilist)
            {
                Console.WriteLine(s);
            }
        }

        [TestMethod]
        public void TestDirectoryInfo()
        {
            //DirectoryInfo di=new DirectoryInfo(@"\\192.168.33.200\Users\Public\Update");
            // var dirs = di.GetDirectories();
            // foreach (DirectoryInfo dir in dirs)
            // {
            //     Console.WriteLine(dir.FullName);
            // }
            string a = NetworkConnection.Connect(@"\\192.168.33.200\Update", "dir", "123");
            //int a = NetworkConnection.Disconnect("B");
            Console.WriteLine(a);
        }

        [TestMethod]
        public void TestCmdNetUse()
        {
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"net use " + @"\\192.168.33.200" + " /User:" + "dir" + " " + "123" + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                proc.WaitForExit();
                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (string.IsNullOrEmpty(errormsg))
                {
                }
                else
                {
                    throw new Exception(errormsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
        }

        [TestMethod]
        public void TestCmdNetUserDisconnect()
        {
            string local = @"\\192.168.33.200";
            StringBuilder msg = new StringBuilder();
            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"net use";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                proc.WaitForExit();

                //状态 本地        远程 网络
                //-------------------------------------------------------------------------------
                //已断开连接             \\192.168.33.200\IPC$     Microsoft Windows Network
                //命令成功完成。
                do
                {
                    var result = proc.StandardOutput.ReadLine();
                    if (result == null)
                    {
                        break;
                    }
                    if (result.Contains(local))
                    {
                        int a = result.IndexOf(local, StringComparison.Ordinal);
                        int c = result.IndexOf(" ", a, StringComparison.Ordinal);
                        msg.Append(result.Substring(a, c - a));
                        break;
                    }
                } while (true);
                proc.StandardError.Close();
                if (msg.Length <= 0)
                {
                    return;
                }

              
            }
            using (Process proc=new Process())
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.StandardInput.WriteLine($"net use {msg} /delete");
                proc.StandardInput.WriteLine("exit");
                proc.WaitForExit();
                var error = proc.StandardError.ReadToEnd();
            }
        }

        [TestMethod]
        public void ReadNetUserFile()
        {
            using (StreamReader sr = new StreamReader(@"\\192.168.33.200\Update\data.ud"))
            {
                String line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);

                }
            }
        }

        [TestMethod]
        public void TestDir()
        {
            //var s = Directory.GetDirectories("C:\\");
            //foreach (string s1 in s)
            //{
            //    Console.WriteLine(s1);
            //}
            Console.WriteLine(File.Exists(@"F:\KuGou\\Lyric\1.krc"));
        }

        [TestMethod]
        public void GetMd5()
        {
            FileStream fileStream = new FileStream(@"E:\Work\Zhongs\Anmeng.Queue\Anmeng.Queue\bin\Release\Anmeng.Queue.exe", FileMode.Open, FileAccess.Read);
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash((Stream)fileStream);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in hash)
                stringBuilder.Append(num.ToString("x2"));
            fileStream.Close();
            Console.WriteLine(stringBuilder.ToString());}
    }
}
