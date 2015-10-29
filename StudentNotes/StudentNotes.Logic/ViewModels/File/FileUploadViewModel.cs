using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.Logic.ViewModels.File
{
    public class FileUploadViewModel
    {
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public byte[] FileContent { get; set; }
    }
}
