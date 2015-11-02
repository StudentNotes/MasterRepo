using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Base;

namespace StudentNotes.FileTransferManager.Abstraction
{
    public interface IDownloadBehavior<T>
    {
        T DownloadFile(FileServerFile file, FileServer server, FileServerUser user);
    }
}
