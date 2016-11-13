using System;
using System.IO;
using System.Net;

class ftp
{
    private string host = null;
    private string user = null;
    private string pass = null;
    private FtpWebRequest ftpRequest = null;
    private FtpWebResponse ftpResponse = null;
    private Stream ftpStream = null;
    private int bufferSize = 2048;

    public ftp(string hostIP, string userName, string password) { host = hostIP; user = userName; pass = password; }

    public void download(string remoteFile, string localFile)
    {
        try
        {
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpStream = ftpResponse.GetResponseStream();
            FileStream localFileStream = new FileStream(localFile, FileMode.Create);
            byte[] byteBuffer = new byte[bufferSize];
            int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
            try
            {
                while (bytesRead > 0)
                {
                    localFileStream.Write(byteBuffer, 0, bytesRead);
                    bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            localFileStream.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    public void upload(string remoteFile, string localFile)
    {
        try
        {
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpStream = ftpRequest.GetRequestStream();
            FileStream localFileStream = new FileStream(localFile, FileMode.Create);
            byte[] byteBuffer = new byte[bufferSize];
            int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
            try
            {
                while (bytesSent != 0)
                {
                    ftpStream.Write(byteBuffer, 0, bytesSent);
                    bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            localFileStream.Close();
            ftpStream.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    public void delete(string deleteFile)
    {
        try
        {
            ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    public void rename(string currentFileNameAndPath, string newFileName)
    {
        try
        {
            ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + currentFileNameAndPath);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.Rename;
            ftpRequest.RenameTo = newFileName;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    public void createDirectory(string newDirectory)
    {
        try
        {
            ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + newDirectory);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
            ftpRequest = null;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return;
    }

    public string getFileCreatedDateTime(string fileName)
    {
        try
        {
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpStream = ftpResponse.GetResponseStream();
            StreamReader ftpReader = new StreamReader(ftpStream);
            string fileInfo = null;
            try { fileInfo = ftpReader.ReadToEnd(); }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            return fileInfo;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return "";
    }

    public string getFileSize(string fileName)
    {
        try
        {
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpStream = ftpResponse.GetResponseStream();
            StreamReader ftpReader = new StreamReader(ftpStream);
            string fileInfo = null;
            try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            return fileInfo;
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return "";
    }

    public string[] directoryListSimple(string directory)
    {
        try
        {
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpStream = ftpResponse.GetResponseStream();
            StreamReader ftpReader = new StreamReader(ftpStream);
            string directoryRaw = null;
            try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return new string[] { "" };
    }

    public string[] directoryListDetailed(string directory)
    {
        try
        {
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpStream = ftpResponse.GetResponseStream();
            StreamReader ftpReader = new StreamReader(ftpStream);
            string directoryRaw = null;
            try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        return new string[] { "" };
    }
}
