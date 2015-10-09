using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotesFileTransferManager.Abstraction;

namespace StudentNotesFileTransferManager.FtpClient.FileTypes
{
    public class CommonFile : IFile
    {
        public CommonFile(string name, string path)
        {
            Name = name;
            Path = path;
            Size = 0;
            Content = new byte[]{0};
        }

        public CommonFile(string name, string path, double size, byte[] content)
        {
            Name = name;
            Path = path;
            Size = size;
            Content = content;
        }

        public string Name { get; private set; }
        public double Size { get; private set; }
        public byte[] Content { get; private set; }
        public string Path { get; private set; }
    }
}
