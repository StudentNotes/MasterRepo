using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.File
{
    public class AccessedNotesViewModel
    {
        public ResponseViewModelBase Response { get; set; }
        public List<Note> Notes { get; set; }
        public List<SimpleUserModel> Owners { get; set; } 

        public AccessedNotesViewModel()
        {
            Response = new ResponseMessageViewModel();
            Notes = new List<Note>();
            Owners = new List<SimpleUserModel>();
        }
    }
}
