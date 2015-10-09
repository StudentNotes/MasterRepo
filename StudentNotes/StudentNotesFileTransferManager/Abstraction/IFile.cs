using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotesFileTransferManager.Abstraction
{
    public interface IFile
    {
        string Name { get; }
        double Size { get; }
        byte[] Content { get; }
        string Path { get; }
    }
}
