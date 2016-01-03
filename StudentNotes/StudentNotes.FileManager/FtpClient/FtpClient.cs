using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileManager.Base;

namespace StudentNotes.FileManager.FtpClient
{
    public class FtpClient : FileServerClient
    {
        public FtpClient(string serverUrl, string login, string password) : base(serverUrl, login, password)
        {
            DownloadBehavior = new FtpDownloadBehavior();
            UploadBehavior = new FtpUploadBehavior();
            DirectoryTreeBehavior = new FtpDirectoryTreeBehavior();
            DeleteBehavior = new FtpDeleteBehavior();
            CurrentLocation = "/FTP";
        }

        public override string GetRequestPath(string lastNode)
        {
            return $"ftp://{ServerUrl}{CurrentLocation}/{lastNode}";
        }

        public override string GetRequestPath()
        {
            return $"ftp://{ServerUrl}{CurrentLocation}/";
        }

        public override void GoToRootDir()
        {
            CurrentLocation = "/FTP";
        }

        public override string ToString()
        {
            return $"Ftp user credentials\nlogin: {Login}\npassword: {Password}\nserver address: {ServerUrl}";
        }
    }
}
