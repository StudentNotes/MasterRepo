using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotesDal.LogicInterfaces;

namespace StudentNotesDal.LogicModels
{
    public class Note : INote
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string Size { get; set; }
        public string UploadDate { get; set; }
        public bool IsShared { get; set; }
        public List<string> Tags { get; set; }


        public int UploadNote()
        {
            throw new NotImplementedException();
        }

        public int DownloadNote()
        {
            throw new NotImplementedException();
        }

        public int DeleteNote()
        {
            throw new NotImplementedException();
        }

        public int MoveNote()
        {
            throw new NotImplementedException();
        }

        public int AddTag(string tag)
        {
            throw new NotImplementedException();
        }

        public int RemoveTag(string tag)
        {
            throw new NotImplementedException();
        }
    }
}
