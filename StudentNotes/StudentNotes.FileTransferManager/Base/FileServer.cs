using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.FileTransferManager.Base
{
    public abstract class FileServer
    {
        public string FileDestination { get; private set; }
        public string ServerUrl { get; private set; }
        public string CurrentLocation { get; set; }

        protected FileServer(string serverUrl)
        {
            ServerUrl = serverUrl;
            FileDestination = "/";
            CurrentLocation = "/FTP";
        }

        protected FileServer(string serverUrl, string fileDestination)
        {
            ServerUrl = serverUrl;
            FileDestination = fileDestination;
            CurrentLocation = "/FTP";
        }

        public abstract override string ToString();
    }
}
