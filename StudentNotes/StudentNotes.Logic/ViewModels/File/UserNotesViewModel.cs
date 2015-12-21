using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.File
{
    public class UserNotesViewModel
    {
        public List<Note> UserNotesList { get; private set; }
        public ResponseViewModelBase Response { get; set; }

        public UserNotesViewModel()
        {
            Response = new ResponseMessageViewModel();
            UserNotesList = new List<Note>();
        }
    }
}
