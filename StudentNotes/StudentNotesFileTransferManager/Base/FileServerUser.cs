using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Abstraction;

namespace StudentNotesFileTransferManager.Base
{
    public abstract class FileServerUser
    {
        public string login;
        public string password;
        protected FileServer server;

        protected IDownloadBehavior<byte[]> downloadBehavior;
        protected IUploadBehavior<int> uploadBehavior;

        protected FileServerUser(string login, string password, FileServer server)
        {
            this.login = login;
            this.password = password;
            this.server = server;
        }

        public abstract override string ToString();

        public byte[] DownloadFile(IFile file)
        {
            return downloadBehavior.DownloadFile(file, server, this);
        }

        public int UploadFile(IFile file)
        {
            return uploadBehavior.UploadFile(file, server, this);
        }
    }
}
