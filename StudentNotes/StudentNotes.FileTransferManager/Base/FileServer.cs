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
        public string FileDestination { get; protected set; }
        public string ServerUrl { get; protected set; }
        public string CurrentLocation { get; set; }

        protected FileServer(string serverUrl)
        {
            ServerUrl = serverUrl;            
        }

        protected FileServer(string serverUrl, string fileDestination)
        {
            ServerUrl = serverUrl;
            FileDestination = fileDestination;
        }

        public abstract override string ToString();
    }
}
