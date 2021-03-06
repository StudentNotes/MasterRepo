﻿using System;
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

        protected FileServerClient(string serverUrl, string login, string password)
        {
            ServerUrl = serverUrl;
            Login = login;
            Password = password;
        }
        public virtual void ChangeCredentials(string serverUrl, string login, string password)
        {
            ServerUrl = serverUrl;
            Login = login;
            Password = password;
        }

        public abstract string GetRequestPath(string lastNode);

        public abstract string GetRequestPath();

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

        public bool CheckIfFileExists(string name)
        {
            return DirectoryTreeBehavior.FileOrDirectoryAlreadyExists(name, this);
        }

        public string GetNewName(string oldName)
        {
            return DirectoryTreeBehavior.GetNewNameForFile(oldName, this);
        }

        public abstract void GoToRootDir();

        public abstract override string ToString();
    }
}
