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
    public class FtpUploadBehavior : FtpBehaviorBase, IUploadBehavior<int>
    {
        public FtpResponseCode ServerResponseMessage { get; private set; }

        public int UploadFile(File file, FileServer server, FileServerUser user)
        {
            if (file.Size == 0)
            {
                file.LoadContentFromDrive();
            }
            

            string requestPath = string.Format(@"ftp://{0}{1}/{2}", server.ServerUrl, server.CurrentLocation, file.Name);
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            
            ftpRequest.Credentials = new NetworkCredential(user.login, user.password);

            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            ftpRequest.ContentLength = file.Content.Length;

            using (Stream ftpUploadStream = ftpRequest.GetRequestStream())
            {
                ftpUploadStream.Write(file.Content, 0, file.Content.Length);
            }

            var response = ((FtpWebResponse) ftpRequest.GetResponse()).StatusDescription;
            if (base.GetResponseCode(response) == (int) FtpResponseCode.FileUploaded)
            {
                ServerResponseMessage = FtpResponseCode.FileUploaded;
                return (int)ServerResponseMessage;
            }
            ServerResponseMessage = FtpResponseCode.GlobalError;
            return (int)ServerResponseMessage;
        }
    }
}
