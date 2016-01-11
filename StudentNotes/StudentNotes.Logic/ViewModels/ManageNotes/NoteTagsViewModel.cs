using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.ManageNotes
{
    public class NoteTagsViewModel
    {
        public ResponseViewModelBase Response { get; set; }

        public int NoteId { get; set; }
        public string Tags { get; set; }

        public NoteTagsViewModel()
        {
            Response = new ResponseMessageViewModel();
        }
    }
}
