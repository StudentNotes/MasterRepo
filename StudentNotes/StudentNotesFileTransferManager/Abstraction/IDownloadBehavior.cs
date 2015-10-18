using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Base;

namespace StudentNotesFileTransferManager.Abstraction
{
    public interface IDownloadBehavior<T>
    {
        T DownloadFile(File file, FileServer server, FileServerUser user);
    }
}
