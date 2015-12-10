using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.Authorization;

namespace StudentNotes.Web.RequestViewModels.Group
{
    public class DeleteGroupNoteRequest : RequestModelBase
    {
        public int NoteId { get; set; }
        public int GroupId { get; set; }
        public int SemesterSubjectId { get; set; }

        public override ResponseViewModelBase Validate()
        {
            LoginViewModel response = new LoginViewModel();

            IsValid = true;

            return response;
        }
    }
}
