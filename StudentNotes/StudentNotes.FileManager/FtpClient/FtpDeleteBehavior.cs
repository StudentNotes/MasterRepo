using System.Net;
using System.Threading.Tasks;
using StudentNotes.FileManager.Abstraction;
using StudentNotes.FileManager.Base;

namespace StudentNotes.FileManager.FtpClient
{
    public class FtpDeleteBehavior : IDeleteBehavior
    {
        public async Task DeleteFile(string name, FileServerClient client)
        {
            var requestPath = client.GetRequestPath(name);
            var ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            ftpRequest.Credentials = new NetworkCredential(client.Login, client.Password);
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            var response = (FtpWebResponse)await ftpRequest.GetResponseAsync();

            client.GetResponseCode(response.StatusDescription);
        }

        public async Task DeleteDirectory(string name, FileServerClient client)
        {
            var requestPath = client.GetRequestPath(name);
            var ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            ftpRequest.Credentials = new NetworkCredential(client.Login, client.Password);
            ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;

            var response = (FtpWebResponse) await ftpRequest.GetResponseAsync();

            client.GetResponseCode(response.StatusDescription);
        }
    }
}
