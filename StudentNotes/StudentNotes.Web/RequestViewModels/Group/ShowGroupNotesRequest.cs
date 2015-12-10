using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.Authorization;

namespace StudentNotes.Web.RequestViewModels.Group
{
    public class ShowGroupNotesRequest : RequestModelBase
    {
        public int SemesterSubjectId { get; set; }
        public int SemesterId { get; set; }
        public int GroupId { get; set; }

        public override ResponseViewModelBase Validate()
        {
            LoginViewModel response = new LoginViewModel();

            IsValid = true;

            return response;
        }
    }
}
