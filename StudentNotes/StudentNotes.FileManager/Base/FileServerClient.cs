using System;
using System.Threading.Tasks;
using StudentNotes.FileManager.Abstraction;
using StudentNotes.FileManager.Consts;

namespace StudentNotes.FileManager.Base
{
    public abstract class FileServerClient
    {
        public ServerResponseCode ServerResponse { get; set; }
        public string ServerUrl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string CurrentLocation { get; set; }
        public int PercentDone { get; set; }

        protected IDownloadBehavior<File> DownloadBehavior;
        protected IUploadBehavior UploadBehavior;
        protected IDeleteBehavior DeleteBehavior;
        protected IDirectoryTreeBehavior DirectoryTreeBehavior;

        public FileServerClient(string serverUrl, string login, string password)
        {
            ServerUrl = serverUrl;
            Login = login;
            Password = password;
            CurrentLocation = "/FTP";
        }
        public virtual void ChangeCredentials(string serverUrl, string login, string password)
        {
            ServerUrl = serverUrl;
            Login = login;
            Password = password;
        }

        public string GetRequestPath(string lastNode)
        {
            return $"ftp://{ServerUrl}{CurrentLocation}/{lastNode}";
        }

        public string GetRequestPath()
        {
            return $"ftp://{ServerUrl}{CurrentLocation}/";
        }

        public void GetResponseCode(string responseMessage)
        {
            ServerResponse = (ServerResponseCode)int.Parse(responseMessage.Substring(0, 3));
        }

        public File DownloadFile(string name)
        {
            return DownloadBehavior.DownloadFile(name, this);
        }

        public async Task UploadFile(File file)
        {
            await UploadBehavior.UploadFile(file, this);
        }

        public async Task DeleteDirectory(string name)
        {
            await DeleteBehavior.DeleteDirectory(name, this);
        }

        public async Task DeleteFile(string name)
        {
            await DeleteBehavior.DeleteFile(name, this);
        }

        public void GoToOrCreatePath(string path)
        {
            DirectoryTreeBehavior.GoToOrCreatePath(path, this);
        }

        public void GoToPath(string path)
        {
            DirectoryTreeBehavior.GoToPath(path, this);
        }

        public abstract override string ToString();
    }
}
