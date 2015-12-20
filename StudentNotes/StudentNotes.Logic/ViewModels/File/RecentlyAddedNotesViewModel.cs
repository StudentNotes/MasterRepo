using System;
using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.File
{
    public class RecentlyAddedNotesViewModel
    {
        //public int NoteId { get; set; }
        //public string Name { get; set; }
        //public string Category { get; set; }
        //public string Size { get; set; }
        //public string DestinationPath { get; set; }
        //public DateTime UploadDate { get; set; }

        public ResponseViewModelBase Response { get; set; }
        public List<Note> Notes { get; set; }

        public RecentlyAddedNotesViewModel()
        {
            Response = new ResponseMessageViewModel();
            Notes = new List<Note>();
        }
    }
}
