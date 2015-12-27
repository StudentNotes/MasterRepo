using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Abstraction;
using StudentNotes.FileTransferManager.Base;
using StudentNotes.FileTransferManager.Consts;

namespace StudentNotes.FileTransferManager.HttpClient.HttpBehavior
{
    public class HttpUploadBehavior : IUploadBehavior<int>
    {
        public HttpResponseCode ServerResponseMessage { get; private set; }

        public Task<int> UploadFile(FileServerFile file, FileServer server, FileServerUser user)
        {
            throw new NotImplementedException();
        }
    }
}
