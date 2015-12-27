using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Base;

namespace StudentNotes.FileTransferManager.HttpClient
{
    public class HttpUser : FileServerUser
    {
        public HttpUser(string login, string password, FileServer server) : base(login, password, server)
        {
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
