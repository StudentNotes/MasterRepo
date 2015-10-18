using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Abstraction;
using StudentNotesFileTransferManager.Base;
using File = StudentNotesFileTransferManager.Base.File;

namespace StudentNotesFileTransferManager.FtpClient.TransferBehavior
{
    class FtpUploadBehavior : IUploadBehavior<int>
    {
        public string ServerResponseMessage { get; private set; }

        public int UploadFile(File file, FileServer server, FileServerUser user)
        {
            file.LoadContentFromDrive();

            string requestPath = string.Format(@"ftp://{0}{1}", server.ServerUrl, server.FileDestination);
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            
            ftpRequest.Credentials = new NetworkCredential(user.login, user.password);

            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            ftpRequest.ContentLength = file.Content.Length;

            using (Stream ftpUploadStream = ftpRequest.GetRequestStream())
            {
                ftpUploadStream.Write(file.Content, 0, file.Content.Length);
            }

            ServerResponseMessage = ((FtpWebResponse) ftpRequest.GetResponse()).StatusDescription;

            return 0;
        }
    }
}
