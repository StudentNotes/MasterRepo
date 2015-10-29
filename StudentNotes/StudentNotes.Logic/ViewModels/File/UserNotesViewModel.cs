using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.DBModels;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ViewModels.File
{
    public class UserNotesViewModel
    {
        public List<Note> UserNotesList { get; private set; }

        public UserNotesViewModel()
        {
            UserNotesList = new List<Note>();
        }

        public void GetAllUserNotes(int userId)
        {
            using (var context = new StudentNotesContext())
            {
                var userFiles = from f in context.File where f.UserId == userId select f;

                if (!userFiles.Any())
                {
                    return;
                }

                foreach (var file in userFiles)
                {
                    UserNotesList.Add(new Note(file.FileId)
                    {
                        Name = file.Name,
                        Category = file.Category,
                        DestinationPath = file.Path,
                        IsShared = file.IsShared,
                        Size = file.Size,
                        UploadDate = file.UploadDate,
                        Tags = file.FileTags.Split(';').ToList()
                    });
                }
            }
        }

        public void GetPrivateUserNotes(int userId)
        {
            using (var context = new StudentNotesContext())
            {
                var userFiles = from f in context.File where f.UserId == userId && f.IsShared == false select f;

                if (!userFiles.Any())
                {
                    return;
                }

                foreach (var file in userFiles)
                {
                    UserNotesList.Add(new Note(file.FileId)
                    {
                        Name = file.Name,
                        Category = file.Category,
                        DestinationPath = file.Path,
                        IsShared = file.IsShared,
                        Size = file.Size,
                        UploadDate = file.UploadDate,
                        Tags = file.FileTags.Split(';').ToList()
                    });
                }
            }
        }
    }
}
