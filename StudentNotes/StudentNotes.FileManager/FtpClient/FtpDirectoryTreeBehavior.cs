using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using StudentNotes.FileManager.Abstraction;
using StudentNotes.FileManager.Base;
using StudentNotes.FileManager.Consts;

namespace StudentNotes.FileManager.FtpClient
{
    public class FtpDirectoryTreeBehavior : IDirectoryTreeBehavior
    {
        public void GoToOrCreatePath(string path, FileServerClient client)
        {
            var pathDirectories = ConvertDirectoryListingToList(path);

            for (int i = 1; i < pathDirectories.Count; i++)
            {
                ChangeCurrentDirectoryOrCreateDirectory(pathDirectories[i], client);

                if (client.ServerResponse == ServerResponseCode.FolderCreated || client.ServerResponse == ServerResponseCode.FolderExists)
                {
                    continue;
                }
                client.ServerResponse = ServerResponseCode.GlobalError;
                return;
            }
            client.ServerResponse = ServerResponseCode.CommandsExecutedSuccessfully;
        }

        public void GoToPath(string path, FileServerClient client)
        {
            var pathDirectories = ConvertDirectoryListingToList(path);

            for (int i = 1; i < pathDirectories.Count; i++)
            {
                ChangeCurrentDirectory(pathDirectories[i], client);
                if (client.ServerResponse == ServerResponseCode.CommandsExecutedSuccessfully)
                {
                    continue;
                }
                client.ServerResponse = ServerResponseCode.GlobalError;
                return;
            }
            client.ServerResponse = ServerResponseCode.CommandsExecutedSuccessfully;
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

        private void ChangeCurrentDirectoryOrCreateDirectory(string directory, FileServerClient client)
        {
            List<string> directories = GetDirectoriesList(client);

            if (!directories.Contains(directory))
            {
                var requestPath = client.GetRequestPath(directory);
                var ftpRequest = (FtpWebRequest) WebRequest.Create(requestPath);
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftpRequest.Credentials = new NetworkCredential(client.Login, client.Password);

                var response = (FtpWebResponse) ftpRequest.GetResponse();
                client.GetResponseCode(response.StatusDescription);
                if (client.ServerResponse == ServerResponseCode.FolderCreated)
                {
                    client.CurrentLocation += ("/" + directory);
                }
            }
            else
            {
                client.CurrentLocation += ("/" + directory);
                client.ServerResponse = ServerResponseCode.FolderExists;
            }
        }

        private void ChangeCurrentDirectory(string directory, FileServerClient client)
        {
            List<string> directories = GetDirectoriesList(client);

            if (!directories.Contains(directory))
            {
                client.ServerResponse = ServerResponseCode.FileDoesntExist;
            }
            else
            {
                client.CurrentLocation += ("/" + directory);
                client.ServerResponse = ServerResponseCode.CommandsExecutedSuccessfully;
            }          
        }

        private List<string> GetDirectoriesList(FileServerClient client)
        {
            var requestPath = client.GetRequestPath();
            var ftpRequest = (FtpWebRequest)WebRequest.Create(requestPath);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            ftpRequest.Credentials = new NetworkCredential(client.Login, client.Password);

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
    }
}
