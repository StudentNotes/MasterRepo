using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.File
{
    public class SearchedNotesViewModel
    {
        public ResponseViewModelBase Response { get; set; }
        public List<Note> Notes { get; set; }

        public SearchedNotesViewModel()
        {
            Response = new ResponseMessageViewModel();
            Notes = new List<Note>();
        }
    }
}
