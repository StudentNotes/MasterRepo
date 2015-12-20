using System.Collections.Generic;
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
    }
}
