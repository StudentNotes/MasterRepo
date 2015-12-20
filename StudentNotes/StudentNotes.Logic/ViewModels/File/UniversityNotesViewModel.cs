using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.File
{
    public class UniversityNotesViewModel
    {
        public ResponseViewModelBase Response { get; set; }
        public List<Note> Notes { get; set; }

        public UniversityNotesViewModel()
        {
            Response = new ResponseMessageViewModel();
            Notes = new List<Note>();
        }
    }
}
