using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.ManageNotes
{
    public class AllNotesDetailsViewModel
    {
        public int NoteType { get; set; }

        public Note Note { get; set; }
        public ResponseViewModelBase Response { get; set; }
        public SimpleUserModel NoteOwner { get; set; }
        public List<SimpleGroupModel> NoteGroup { get; set; }
        public NoteTagsViewModel NoteTags { get; set; }

        public AllNotesDetailsViewModel()
        {
            Response = new ResponseMessageViewModel();
            NoteOwner = new SimpleUserModel();
            NoteGroup = new List<SimpleGroupModel>();
            Note = new Note();
            NoteTags = new NoteTagsViewModel();
        }
    }
}
