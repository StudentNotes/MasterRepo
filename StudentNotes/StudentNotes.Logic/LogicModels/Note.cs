using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicInterfaces;

namespace StudentNotes.Logic.LogicModels
{
    public class Note : INote
    {
        public int UserId { get; private set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string DestinationPath { get; set; }
        public string Size { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsShared { get; set; }
        public List<string> Tags { get; set; }
        public byte[] Content { get; set; }

        public Note()
        {
            Tags = new List<string>();

        }

        public Note(int userId)
        {
            UserId = userId;
            Tags = new List<string>();
        }

        public int UploadNote()
        {
            throw  new NotImplementedException();
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
