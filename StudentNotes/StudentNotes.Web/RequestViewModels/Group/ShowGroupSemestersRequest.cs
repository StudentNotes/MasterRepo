using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.Authorization;

namespace StudentNotes.Web.RequestViewModels.Group
{
    public class ShowGroupSemestersRequest : RequestModelBase
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public override ResponseViewModelBase Validate()
        {
            LoginViewModel response = new LoginViewModel();

            IsValid = true;

            return response;
        }
    }
}
