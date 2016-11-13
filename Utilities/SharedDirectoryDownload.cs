using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Net;

namespace Utilities
{
    public class SharedDirectoryDownload : IDownload, IDisposable
    {
        private string _remote;
        public bool Connect(string remote,string userName, string passWord)
        {
            this._remote = remote;
            return string.IsNullOrWhiteSpace(NetworkConnection.Connect(remote, userName, passWord));
        }

        public IEnumerable<DirectoryInfo> LoadDirectoryInfo(string path)
        {
            return Directory.GetDirectories(path).Select(a => new DirectoryInfo(a));
        }

        public IEnumerable<FileInfo> LoadFileInfos(string path)
        {
            return Directory.GetFiles(path).Select(a => new FileInfo(a));
        }

        public string LoadFileToString(string file)
        {
            return File.ReadAllText(file);
        }

        public void LoadFileToIEnmuerable(string file, string localFile)
        {
            File.Copy(file,localFile,true);
        }

        public bool Exists(string file)
        {
            return File.Exists(file);
        }

        public void Dispose()
        {
            NetworkConnection.Disconnect(_remote);
        }

        public IEnumerable<string> LoadFileToIEnmuerable(string file)
        {
            return File.ReadLines(file);
        }
    }
}
