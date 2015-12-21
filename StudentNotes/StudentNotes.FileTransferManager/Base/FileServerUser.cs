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

        public async Task<int> UploadFile(FileServerFile file)
        {
            return await UploadBehavior.UploadFile(file, server, this);
        }

        public int DeleteDirectory()
        {
            return DeleteBehavior.DeleteDirectory(server, this);
        }

        public async Task<int> DeleteFile()
        {
            return await DeleteBehavior.DeleteFile(server, this);
        }

        public async Task<int> DeleteFile(string remoteLocation)
        {
            return await DeleteBehavior.DeleteFile(remoteLocation, server, this);
        }

        public int GoToOrCreatePath(string destinationPath)
        {
            return (int)DirectoryBehavior.GoToOrCreatePath(destinationPath);
        }

        public int GoToPath(string destinationPath)
        {
            return (int) DirectoryBehavior.GoToPath(destinationPath);
        }


    }
}
