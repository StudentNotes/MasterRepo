using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Base;

namespace StudentNotes.FileTransferManager.Abstraction
{
    public interface IDeleteBehavior<T>
    {
        Task<T> DeleteFile(FileServer server, FileServerUser user);
        Task<T> DeleteFile(string remoteLocation, FileServer server, FileServerUser user);
        T DeleteDirectory(FileServer server, FileServerUser user);
    }
}
