using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = StudentNotes.FileTransferManager.Base.File;

namespace StudentNotes.FileTransferManager.FtpClient.FileTypes
{
    public class CommonFile : File
    {
        public CommonFile(string path) : base(path)
        {
        }

    }
}
