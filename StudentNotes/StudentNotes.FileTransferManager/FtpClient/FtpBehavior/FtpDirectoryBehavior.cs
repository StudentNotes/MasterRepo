using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using StudentNotes.FileTransferManager.Abstraction;
using StudentNotes.FileTransferManager.Base;
using StudentNotes.FileTransferManager.Consts;

namespace StudentNotes.FileTransferManager.FtpClient.FtpBehavior
{
    public class FtpDirectoryBehavior : FtpBehaviorBase, IServerDirectoryTreeBehavior
    {
        private readonly FileServer _server;
        private string _userLogin;
        private string _userPassword;

        public FtpDirectoryBehavior()
        {
        }

        public FtpDirectoryBehavior(FileServer server, string userLogin, string userPassword)
        {
            _server = server;
            _userLogin = userLogin;
            _userPassword = userPassword;
        }

        public FtpResponseCode GoToOrCreatePath(string destinationPath)
        {
            var destinationDirectoriesTree = ConvertDirectoryListingToList(destinationPath);

            for (int i = 1; i < destinationDirectoriesTree.Count; i++)
            {
                var responseCode = ChangeCurrentDirectoryOrCreateDirectory(destinationDirectoriesTree[i]);
                if (responseCode == FtpResponseCode.FolderCreated || responseCode == FtpResponseCode.FolderExists)
                {
                    continue;
                }
                return FtpResponseCode.GlobalError;
            }
            return FtpResponseCode.CommandsExecutedSuccessfully;
        }

        public FtpResponseCode GoToPath(string destinationPath)
        {
            var destinationDirectoriesTree = ConvertDirectoryListingToList(destinationPath);

            for (int i = 1; i < destinationDirectoriesTree.Count; i++)
            {
                var responseCode = ChangeCurrentDirectory(destinationDirectoriesTree[i]);
                if (responseCode == FtpResponseCode.CommandsExecutedSuccessfully)
                {
                    continue;
                }
                return FtpResponseCode.GlobalError;
            }
            return FtpResponseCode.CommandsExecutedSuccessfully;
        }

        private FtpResponseCode ChangeCurrentDirectory(string directory)
        {
            List<string> directories = GetDirectoriesList();

            if (!directories.Contains(directory))
            {
                return FtpResponseCode.FileDoesntExist;
            }

            _server.CurrentLocation += ("/" + directory);

            return FtpResponseCode.CommandsExecutedSuccessfully;
        }

        private FtpResponseCode ChangeCurrentDirectoryOrCreateDirectory(string directory)
        {
            List<string> directories = GetDirectoriesList();

            if (!directories.Contains(directory))
            {
                var requestPath = string.Format(@"ftp://{0}{1}/{2}", _server.ServerUrl, _server.CurrentLocation, directory);

                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                ftpRequest.Credentials = new NetworkCredential(_userLogin, _userPassword);

                var resp = (FtpWebResponse)ftpRequest.GetResponse();

                if (GetResponseCode(resp.StatusDescription) == (int)FtpResponseCode.FolderCreated)
                {
                    _server.CurrentLocation += ("/" + directory);
                    return FtpResponseCode.FolderCreated;
                }

                return FtpResponseCode.GlobalError;
            }
            _server.CurrentLocation += ("/" + directory);

            return FtpResponseCode.FolderExists;
        }        

        private List<string> GetDirectoriesList()
        {
            var requestPath = string.Format(@"ftp://{0}{1}", _server.ServerUrl, _server.CurrentLocation);

            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;

            ftpRequest.Credentials = new NetworkCredential(_userLogin, _userPassword);

            var response = (FtpWebResponse)ftpRequest.GetResponse();

            using (var responseStream = response.GetResponseStream())
            {
                var responseReader = new StreamReader(responseStream);
                var separator = Environment.NewLine.ToCharArray();
                var result = responseReader.ReadToEnd().Split(separator).ToList();

                result.RemoveAll(IsEmptyString);

                return result.ToList();
            }
        }

        private List<string> ConvertDirectoryListingToList(string currentLocation)
        {
            var directoryTree = currentLocation.Split('/').ToList();
            directoryTree.RemoveAll(IsEmptyString);
            return directoryTree;
        }

        private bool IsEmptyString(string s)
        {
            return s.Equals("");
        }

    }
}
