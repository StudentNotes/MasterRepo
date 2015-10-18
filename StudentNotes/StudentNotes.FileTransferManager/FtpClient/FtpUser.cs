using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Base;
using StudentNotesFileTransferManager.FtpClient.TransferBehavior;

namespace StudentNotesFileTransferManager.FtpClient
{
    public class FtpUser : FileServerUser
    {
        public FtpUser(string login, string password, FileServer server) : base(login, password, server)
        {
            downloadBehavior = new FtpDownloadBehavior();
            uploadBehavior = new FtpUploadBehavior();
        }

        public override string ToString()
        {
            return string.Format("Ftp user credentials\nlogin: {0}\npassword: {1}\nserver address: {2}", 
                login, password, server.ServerUrl);
        }
    }
}
