using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class UpdateModule
    {
        private IDownload _download;
        private readonly string _remote;
        private readonly string _local;
        private readonly string _localUd;
        private readonly string _remoteUd;

        public UpdateModule(IDownload download,string remote,string local)
        {
            _download = download;
            _remote = remote;
            _remoteUd = remote + "\\data.ud";
            _local = local;
            _localUd = local + "\\data.ud";
        }
        
        /// <summary>
        /// 检查是否有升级
        /// </summary>
        /// <returns></returns>
        public bool CheckUpdate()
        {
            var locExists = File.Exists(_localUd);
            var reomExists = File.Exists(_remoteUd);
            if (locExists && reomExists)
            {
                string localUdString = File.ReadAllText(_localUd).Trim();
                if (string.IsNullOrWhiteSpace(localUdString))
                {
                    return true;
                }
                var remoteUdString = _download.LoadFileToIEnmuerable(_remoteUd).ToList();
                if (remoteUdString.Count == 0)
                {
                    return false;
                }
                var find = remoteUdString.FirstOrDefault(a => a == localUdString && !string.IsNullOrWhiteSpace(a));
                var last = remoteUdString.LastOrDefault(a => !string.IsNullOrWhiteSpace(a));
                if (string.IsNullOrWhiteSpace(find))
                {
                    return true;
                }
                if (string.IsNullOrWhiteSpace(last))
                {
                    return false;
                }
                return !find.Equals(last);
            }
            else if (reomExists)
            {
                return true;
            }else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新程序
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            try
            {
                if (!_download.Exists(_remoteUd))
                {
                    return false; 
                }
                var remoteUdString = _download.LoadFileToIEnmuerable(_remoteUd).ToList();
                if (remoteUdString.Count == 0)
                {
                    return false;
                }
                if (!File.Exists(_localUd))
                {
                    File.Create(_localUd).Dispose();
                }
                string localUdString = File.ReadAllText(_localUd).Trim();
            
                #region 查找version

                List<string> version = new List<string>();
                if (string.IsNullOrWhiteSpace(localUdString))
                {//本地版本为空，则去服务器第一个版本
                    foreach (var item in remoteUdString)
                    {
                        version.Add(item);
                    }
                }
                else
                {
                    bool fined = false;
                    //查找下一个版本
                    foreach (string itemString in remoteUdString)
                    {
                        if (itemString == localUdString)
                        {
                            fined = true;
                        }
                        else
                        {
                            if (fined)
                            {
                                version.Add(itemString);
                            }
                        }
                    }
                    if (fined == false)
                    {//没有匹配的记录，则从第一个开始更新
                        version.Add(remoteUdString.FirstOrDefault());
                    }
                }
                if (!version.Any())
                {
                    return false;
                }

                #endregion

                foreach (string ver in version)
                {
                    Console.WriteLine("更新版本:{0}", ver);
                    var result = CopyToLocal(ver);
                    if (result)
                    {
                        File.WriteAllText(_localUd, ver);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        private bool CopyToLocal(string verson)
        {
            return ReomteDirToLocalDir(_remote + "\\" + verson, _local);
        }

        private bool ReomteDirToLocalDir(string reomtePath,string localPath)
        {
            try
            {
                var fileInfos=_download.LoadFileInfos(reomtePath);
                foreach (FileInfo info in fileInfos)
                {
                    if (info.Extension == "ud")
                    {
                        continue;
                    }
                    string newFile = localPath + "\\" + info.Name + ".fud";
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    File.Copy(info.FullName, newFile, true);
                }
                var dirInfos = _download.LoadDirectoryInfo(reomtePath);
                foreach (DirectoryInfo info in dirInfos)
                {
                    if (!ReomteDirToLocalDir(reomtePath + "\\" + info.Name, localPath + "\\" + info.Name))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

    }
}
