using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotesFileTransferManager.Base
{
    public abstract class FileServer
    {
        public string ServerUrl
        {
            get;
            private set;
        }

        protected FileServer(string serverUrl)
        {
            ServerUrl = serverUrl;
        }

        public abstract override string ToString();
    }
}
