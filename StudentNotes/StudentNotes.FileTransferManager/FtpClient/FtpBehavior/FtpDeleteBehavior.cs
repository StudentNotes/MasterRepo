using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Abstraction;
using StudentNotes.FileTransferManager.Base;
using StudentNotes.FileTransferManager.Consts;
using File = StudentNotes.FileTransferManager.Base.FileServerFile;

namespace StudentNotes.FileTransferManager.FtpClient.FtpBehavior
{
    public class FtpDeleteBehavior : FtpBehaviorBase, IDeleteBehavior<int>
    {
        public int DeleteFile(FileServer server, FileServerUser user)
        {
            string requestPath = string.Format(@"ftp://{0}/{1}", server.ServerUrl, server.FileDestination);
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);

            ftpRequest.Credentials = new NetworkCredential(user.login, user.password);

            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            var serverResponseMessage = ((FtpWebResponse)ftpRequest.GetResponse()).StatusDescription;
            if (GetResponseCode(serverResponseMessage) == (int) FtpResponseCode.FileDeleted)
            {
                return (int) FtpResponseCode.FileDeleted;
            }

            return (int)FtpResponseCode.GlobalError;
        }

        public int DeleteFile(string remoteLocation, FileServer server, FileServerUser user)
        {
            string requestPath = string.Format(@"ftp://{0}/{1}", server.ServerUrl, remoteLocation);
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);

            ftpRequest.Credentials = new NetworkCredential(user.login, user.password);

            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            var serverResponseMessage = ((FtpWebResponse)ftpRequest.GetResponse()).StatusDescription;
            if (GetResponseCode(serverResponseMessage) == (int)FtpResponseCode.FileDeleted)
            {
                return (int)FtpResponseCode.FileDeleted;
            }

            return (int)FtpResponseCode.GlobalError;
        }

        public int DeleteDirectory(FileServer server, FileServerUser user)
        {
            string requestPath = string.Format(@"ftp://{0}/{1}", server.ServerUrl, server.FileDestination);
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);

            ftpRequest.Credentials = new NetworkCredential(user.login, user.password);

            ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;

            var serverResponseMessage = ((FtpWebResponse)ftpRequest.GetResponse()).StatusDescription;
            if (GetResponseCode(serverResponseMessage) == (int)FtpResponseCode.FileDeleted)
            {
                return (int)FtpResponseCode.FileDeleted;
            }

            return (int)FtpResponseCode.GlobalError;
        }

    }
}
