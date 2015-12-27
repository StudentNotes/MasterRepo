using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Base;

namespace StudentNotes.FileTransferManager.FtpClient
{
    public class FtpServer : FileServer
    {
        public FtpServer(string serverUrl) : base(serverUrl)
        {
            FileDestination = "/";
            CurrentLocation = "/FTP";
        }

        public FtpServer(string serverUrl, string fileDestination)
            : base(serverUrl, fileDestination)
        {
            CurrentLocation = "/FTP";
        }

        public override string ToString()
        {
            return string.Format("This file server uses the FTP protocol, and it's Url is: {0}", base.ServerUrl);
        }
    }
}
