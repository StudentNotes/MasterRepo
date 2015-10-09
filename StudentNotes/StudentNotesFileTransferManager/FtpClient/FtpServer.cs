using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Base;

namespace StudentNotesFileTransferManager.FtpClient
{
    public class FtpServer : FileServer
    {
        public FtpServer(string serverUrl) : base(serverUrl)
        {
        }

        public override string ToString()
        {
            return string.Format("This file server uses the FTP protocol, and it's Url is: {0}", base.ServerUrl);
        }
    }
}
