using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Base;

namespace StudentNotesFileTransferManager.Abstraction
{
    public interface IUploadBehavior<T>
    {
        T UploadFile(File file, FileServer server, FileServerUser user);
    }
}
