using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Abstraction;
using StudentNotesFileTransferManager.Base;

namespace StudentNotesFileTransferManager.FtpClient.TransferBehavior
{
    class FtpUploadBehavior : IUploadBehavior<int>
    {
        public int UploadFile(IFile file, FileServer server, FileServerUser user)
        {
            throw new NotImplementedException();
        }
    }
}
