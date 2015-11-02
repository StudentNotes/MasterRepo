using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = StudentNotes.FileTransferManager.Base.FileServerFile;

namespace StudentNotes.FileTransferManager.FtpClient.FileTypes
{
    public class CommonFile : File
    {
        public CommonFile(string path) : base(path)
        {
        }
        public CommonFile(string name, string path, byte[] content)
            : base(name, path, content)
        {
        }
        public CommonFile(string name, byte[] content)
            : base(name, content)
        {
        }

    }
}
