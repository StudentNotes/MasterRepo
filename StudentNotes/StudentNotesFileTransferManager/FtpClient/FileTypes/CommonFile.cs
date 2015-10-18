using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Abstraction;
using File = StudentNotesFileTransferManager.Base.File;

namespace StudentNotesFileTransferManager.FtpClient.FileTypes
{
    public class CommonFile : File
    {
        public CommonFile(string path) : base(path)
        {
        }

    }
}
