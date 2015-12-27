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
        }

        public override string ToString()
        {
            return $"Ftp user credentials\nlogin: {Login}\npassword: {Password}\nserver address: {ServerUrl}";
        }
    }
}
