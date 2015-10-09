using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Abstraction;
using StudentNotesFileTransferManager.Base;

namespace StudentNotesFileTransferManager.FtpClient.TransferBehavior
{
    public class FtpDownloadBehavior : IDownloadBehavior<byte[]>
    {
        public const int SEGMENT_SIZE = 1024000;

        public byte[] DownloadFile(IFile file, FileServer server, FileServerUser user)
        {
            byte[] downloadedFilePart;
            byte[] completeFile = new byte[0];

            string requestPath = string.Format("ftp://{0}{1}{2}", server.ServerUrl, file.Path, file.Name);
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            ftpRequest.Credentials = new NetworkCredential(user.login, user.password);

            FtpWebResponse ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();

            Stream ftpDownloadStream = ftpResponse.GetResponseStream();
            //StreamReader reader = new StreamReader(responseStream);
            //BinaryReader reader = new BinaryReader(ftpDownloadStream);
            using (BinaryReader reader = new BinaryReader(ftpDownloadStream))
            {
                downloadedFilePart = reader.ReadBytes(SEGMENT_SIZE);

                while (downloadedFilePart.Length > 0)
                {
                    if (downloadedFilePart.Length < SEGMENT_SIZE)
                    {
                        DumpData(downloadedFilePart, ref completeFile);
                        break;
                    }

                    DumpData(downloadedFilePart, ref completeFile);
                    downloadedFilePart = reader.ReadBytes(SEGMENT_SIZE);

                    
                }

                reader.Close();
            }

            string statusDescription = ftpResponse.StatusDescription;

            ftpResponse.Close();

            return completeFile;
        }

        private void DumpData(byte[] sourceBytes, ref byte[] allBytes)
        {
            int destinationArrayLength = sourceBytes.Length + allBytes.Length;
            byte[] destinationArray = new byte[destinationArrayLength];

            for (int i = 0; i < allBytes.Length; i++)
            {
                destinationArray[i] = allBytes[i];
            }

            //  Appending the sourceBytes to the allBytes array!
            for (int i = allBytes.Length, j = 0; i < destinationArray.Length; i++, j++)
            {
                destinationArray[i] = sourceBytes[j];
            }

            allBytes = destinationArray;
        }
    }
}
