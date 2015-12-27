using System.IO;
using System.Net;
using StudentNotes.FileManager.Abstraction;
using StudentNotes.FileManager.Base;
using File = StudentNotes.FileManager.Base.File;

namespace StudentNotes.FileManager.FtpClient
{
    public class FtpDownloadBehavior : IDownloadBehavior<File>
    {
        private const int SEGMENT_SIZE = 1024000;

        public File DownloadFile(string name, FileServerClient client)
        {
            var file = new File(name);

            string requestPath = client.GetRequestPath(name);

            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            ftpRequest.Credentials = new NetworkCredential(client.Login, client.Password);
            var ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();

            using (var ftpDownloadStream = ftpResponse.GetResponseStream())
            {
                using (var reader = new BinaryReader(ftpDownloadStream))
                {
                    var downloadedFilePart = reader.ReadBytes(SEGMENT_SIZE);

                    while (downloadedFilePart.Length > 0)
                    {
                        if (downloadedFilePart.Length < SEGMENT_SIZE)
                        {
                            file.AppendBytes(downloadedFilePart);
                            break;
                        }
                        file.AppendBytes(downloadedFilePart);
                        downloadedFilePart = reader.ReadBytes(SEGMENT_SIZE);
                    }
                }
            }
            client.GetResponseCode(ftpResponse.StatusDescription);
            ftpResponse.Close();

            return file;
        }
    }
}
