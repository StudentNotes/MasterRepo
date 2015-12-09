using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicAbstraction;
using File = StudentNotes.Repositories.DbModels.File;

namespace StudentNotes.Logic.LogicModels
{
    public class Note : INote
    {
        public int UserId { get; private set; }
        public int NoteId { get; set; }

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

        public Note(File file)
        {
            UserId = file.UserId;
            NoteId = file.FileId;
            Name = file.Name;
            Category = file.Category;
            DestinationPath = file.Path;
            Size = file.Size;
            UploadDate = file.UploadDate;
            IsShared = file.IsShared;
            Tags = DenormalizeTags(file.FileTags);
        }

        public Note(int userId)
        {
            UserId = userId;
            Tags = new List<string>();
        }

        private List<string> DenormalizeTags(string tagString)
        {
            var splitTagString = tagString.Split(';').ToList();

            splitTagString.RemoveAll(IsEmptyString);

            return splitTagString;
        }

        private bool IsEmptyString(string s)
        {
            return s.Equals("");
        }
    }
}
