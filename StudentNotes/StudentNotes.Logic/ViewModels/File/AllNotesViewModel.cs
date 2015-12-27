using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.File
{
    public class AllNotesViewModel
    {
        public ResponseViewModelBase Response { get; set; }
        public List<Note> Notes { get; set; }

        public AllNotesViewModel()
        {
            Response = new ResponseMessageViewModel();
            Notes = new List<Note>();
        }
    }
}
