using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Base;
using StudentNotes.FileTransferManager.Consts;
using StudentNotes.FileTransferManager.FtpClient.FtpBehavior;

namespace StudentNotes.FileTransferManager.FtpClient
{
    public class FtpUser : FileServerUser
    {
        public FtpUser(string login, string password, FileServer server) : base(login, password, server)
        {
            DownloadBehavior = new FtpDownloadBehavior();
            UploadBehavior = new FtpUploadBehavior();
            DeleteBehavior = new FtpDeleteBehavior();
            DirectoryBehavior = new FtpDirectoryBehavior(server, login, password);
        }
        

        public override string ToString()
        {
            return string.Format("Ftp user credentials\nlogin: {0}\npassword: {1}\nserver address: {2}", 
                login, password, server.ServerUrl);
        }
    }
}
