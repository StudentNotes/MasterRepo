using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Abstraction;

namespace StudentNotes.FileTransferManager.Base
{
    public abstract class FileServerUser
    {
        public string login;
        public string password;
        protected FileServer server;

        protected IDownloadBehavior<byte[]> downloadBehavior;
        protected IUploadBehavior<int> uploadBehavior;
        protected IDeleteBehavior<int> deleteBehavior; 

        protected FileServerUser(string login, string password, FileServer server)
        {
            this.login = login;
            this.password = password;
            this.server = server;
        }

        public abstract override string ToString();

        public byte[] DownloadFile(File file)
        {
            return downloadBehavior.DownloadFile(file, server, this);
        }

        public int UploadFile(File file)
        {
            return uploadBehavior.UploadFile(file, server, this);
        }

        public int DeleteFile()
        {
            return deleteBehavior.DeleteFile(server, this);
        }

        public int DeleteFile(string remoteLocation)
        {
            return deleteBehavior.DeleteFile(remoteLocation, server, this);
        }

    }
}
