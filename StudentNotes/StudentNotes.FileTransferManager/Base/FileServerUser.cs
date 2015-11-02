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

        protected IDownloadBehavior<byte[]> DownloadBehavior;
        protected IUploadBehavior<int> UploadBehavior;
        protected IDeleteBehavior<int> DeleteBehavior;
        protected IServerDirectoryTreeBehavior DirectoryBehavior;

        protected FileServerUser(string login, string password, FileServer server)
        {
            this.login = login;
            this.password = password;
            this.server = server;
        }

        public abstract override string ToString();

        public byte[] DownloadFile(FileServerFile file)
        {
            return DownloadBehavior.DownloadFile(file, server, this);
        }

        public int UploadFile(FileServerFile file)
        {
            return UploadBehavior.UploadFile(file, server, this);
        }

        public int DeleteDirectory()
        {
            return DeleteBehavior.DeleteDirectory(server, this);
        }

        public int DeleteFile()
        {
            return DeleteBehavior.DeleteFile(server, this);
        }

        public int DeleteFile(string remoteLocation)
        {
            return DeleteBehavior.DeleteFile(remoteLocation, server, this);
        }

        public int GoToOrCreatePath(string destinationPath)
        {
            return (int)DirectoryBehavior.GoToOrCreatePath(destinationPath);
        }


    }
}
