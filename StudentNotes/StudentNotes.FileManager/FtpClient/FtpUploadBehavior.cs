using System.IO;
using System.Net;
using System.Threading.Tasks;
using StudentNotes.FileManager.Abstraction;
using StudentNotes.FileManager.Base;
using StudentNotes.FileManager.Consts;
using File = StudentNotes.FileManager.Base.File;

namespace StudentNotes.FileManager.FtpClient
{
    public class FtpUploadBehavior : IUploadBehavior
    {
        public async Task UploadFile(File file, FileServerClient client)
        {
            var requestPath = client.GetRequestPath(file.Name);
            var ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            ftpRequest.Credentials = new NetworkCredential(client.Login, client.Password);
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            ftpRequest.ContentLength = file.Content.Length;

            using (Stream ftpUploadStream = ftpRequest.GetRequestStream())
            {
                await ftpUploadStream.WriteAsync(file.Content, 0, file.Content.Length);
            }

            client.GetResponseCode(((FtpWebResponse)ftpRequest.GetResponse()).StatusDescription);
            if (client.ServerResponse != ServerResponseCode.FileUploaded)
            {
                client.ServerResponse = ServerResponseCode.GlobalError;
            }
        }
    }
}
