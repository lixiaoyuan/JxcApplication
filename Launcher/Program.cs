using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Launcher
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        private static string fileName;

        public static int OF_READWRITE = 2;
        public static  int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        static void Main(string[] args)
        {
            reTry:
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (args.Length == 1)
                {
                    KillProcessByName(args[0]);
                }
                replayFile();
                if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
                {
                    Process.Start(fileName);
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor=ConsoleColor.Red;
                Console.WriteLine($"更新失败!异常:\n{e.Message}\n重试请输入【y】加回车键。按任意键退出!");
                while (true)
                {
                    if (Console.ReadLine() == "y")
                    {
                        goto reTry;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        static void KillProcessByName(string name)
        {
            Process[] pros;
            do
            {
                pros = Process.GetProcessesByName(name);
                foreach (var tem in pros)
                {
                    fileName = tem.MainModule.FileName;
                    tem.Kill();
                }
                Thread.Sleep(1000);
                pros = Process.GetProcessesByName(name);
                if (pros.Length != 0)
                {
                    Console.WriteLine($"请手动退出【{name}】程序!");
                    Thread.Sleep(1000);
                }
            } while (pros.Length != 0);

        }

        static void replayFile()
        {
            FileInfo fileInfo = new FileInfo(typeof(Program).Assembly.Location);
            if (fileInfo.Directory != null)
            {
                var files = fileInfo.Directory.GetFiles("*.fud", SearchOption.AllDirectories);
                foreach (FileInfo info in files)
                {
                    Console.WriteLine(info);
                    FileInfo oldFileInfo = new FileInfo(info.FullName.Remove(info.FullName.Length - 4, 4));
                    if (oldFileInfo.Exists)
                    {
                        int i = 10;
                        check:
                        IntPtr vHandle = _lopen(oldFileInfo.FullName, OF_READWRITE | OF_SHARE_DENY_NONE);
                        if (vHandle == HFILE_ERROR)
                        {
                            Thread.Sleep(500);
                            while (i >= 0)
                            {
                                i--;
                                goto check;
                            }
                        }
                        CloseHandle(vHandle);

                        oldFileInfo.Delete();
                    }
                    info.MoveTo(oldFileInfo.FullName);
                }
            }
        }
    }
}
