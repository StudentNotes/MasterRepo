using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Consts;

namespace StudentNotes.FileTransferManager.Abstraction
{
    public interface IServerDirectoryTreeBehavior
    {
        FtpResponseCode GoToOrCreatePath(string destinationPath);
        FtpResponseCode GoToPath(string destinationPath);
    }
}
