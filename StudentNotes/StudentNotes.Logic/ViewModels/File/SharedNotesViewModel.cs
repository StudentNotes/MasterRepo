using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.File
{
    public class SharedNotesViewModel
    {
        public ResponseViewModelBase Response { get; set; }
        public List<SimpleNoteModel> Notes { get; set; }

        public SharedNotesViewModel()
        {
            Response = new ResponseMessageViewModel();
            Notes = new List<SimpleNoteModel>();
        }

    }
}
