using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Utilities.Net
{
    public class NetworkConnection
    {

        public static string Connect(string remotePath, string username, string password)
        {
            try
            {
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = "cmd.exe";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                    //string dosLine = @"net use " + remotePath + " /User:" + username + " " + password + " /PERSISTENT:YES";
                    string dosLine = @"net use " + remotePath ;
                    proc.StandardInput.WriteLine(dosLine);
                    proc.StandardInput.WriteLine("exit");
                    proc.WaitForExit();
                    string errormsg = proc.StandardError.ReadToEnd();
                    proc.StandardError.Close();
                    return errormsg;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static void Disconnect(string localpath)
        {
            try
            {
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
                        if (result.Contains(localpath))
                        {
                            int a = result.IndexOf(localpath, StringComparison.Ordinal);
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
                using (Process proc = new Process())
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
            catch (Exception)
            {
            }
        }
    }
}
