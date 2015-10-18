using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Abstraction;
using StudentNotes.FileTransferManager.Base;
using File = StudentNotes.FileTransferManager.Base.File;

namespace StudentNotes.FileTransferManager.FtpClient.TransferBehavior
{
    public class FtpDeleteBehavior : IDeleteBehavior<int>
    {
        public int DeleteFile(FileServer server, FileServerUser user)
        {
            string requestPath = string.Format(@"ftp://{0}{1}", server.ServerUrl, server.FileDestination);
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);

            ftpRequest.Credentials = new NetworkCredential(user.login, user.password);

            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            string ServerResponseMessage = ((FtpWebResponse)ftpRequest.GetResponse()).StatusDescription;

            return 0;
        }

        public int DeleteFile(string remoteLocation, FileServer server, FileServerUser user)
        {
            string requestPath = string.Format(@"ftp://{0}{1}", server.ServerUrl, remoteLocation);
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);

            ftpRequest.Credentials = new NetworkCredential(user.login, user.password);

            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            string ServerResponseMessage = ((FtpWebResponse)ftpRequest.GetResponse()).StatusDescription;

            return 0;
        }
    }
}
