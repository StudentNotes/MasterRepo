using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotesFileTransferManager.Base
{
    public abstract class FileServer
    {
        public string FileDestination { get; private set; }
        public string ServerUrl { get; private set; }

        protected FileServer(string serverUrl)
        {
            ServerUrl = serverUrl;
            FileDestination = "/";
        }

        protected FileServer(string serverUrl, string fileDestination)
        {
            ServerUrl = serverUrl;
            FileDestination = fileDestination;
        }

        public abstract override string ToString();
    }
}
