using System.IO;
using System.Net;
using StudentNotes.FileTransferManager.Abstraction;
using StudentNotes.FileTransferManager.Base;
using File = StudentNotes.FileTransferManager.Base.FileServerFile;

namespace StudentNotes.FileTransferManager.FtpClient.FtpBehavior
{
    public class FtpDownloadBehavior : IDownloadBehavior<byte[]>
    {
        public const int SEGMENT_SIZE = 1024000;    //  1 MB segment!
        public string ServerResponseMessage { get; private set; }

        public byte[] DownloadFile(File file, FileServer server, FileServerUser user)
        {
            string requestPath = string.Format("ftp://{0}{1}/{2}", server.ServerUrl, server.CurrentLocation, file.Name);

            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            ftpRequest.Credentials = new NetworkCredential(user.login, user.password);

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
                            file.DumpData(downloadedFilePart);
                            break;
                        }
                        file.DumpData(downloadedFilePart);
                        downloadedFilePart = reader.ReadBytes(SEGMENT_SIZE);
                    }
                }
            }

            ServerResponseMessage = ftpResponse.StatusDescription;

            ftpResponse.Close();

            return file.Content;
        }

        
    }
}
