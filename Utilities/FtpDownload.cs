using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class FtpDownload:IDownload
    {
        public bool Connect(string remote, string userName, string passWord)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DirectoryInfo> LoadDirectoryInfo(string path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileInfo> LoadFileInfos(string path)
        {
            throw new NotImplementedException();
        }

        public string LoadFileToString(string file)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> LoadFileToIEnmuerable(string file)
        {
            throw new NotImplementedException();
        }

        public void LoadFileToIEnmuerable(string file, string localFile)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string file)
        {
            throw new NotImplementedException();
        }
    }
}
