using System.Web.WebPages;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Authorization;
using StudentNotes.Web.Models;
using StudentNotes.Web.Models.ResourcesFinderLogic;

namespace StudentNotes.Web.RequestViewModels.Group
{
    public class CreateGroupRequest : RequestModelBase
    {
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public int SemesterId { get; set; }

        public override ResponseViewModelBase Validate()
        {
            LoginViewModel response = new LoginViewModel();

            if (GroupName.IsEmpty())
            {
                response.ErrorList.Add(ResourceKeyResolver.ErrorGroupNameEmpty);
                return response;
            }
            if (NinjectResolver.GetInstance<IGroupService>().GroupInSemesterExists(GroupName, SemesterId))
            {
                response.ErrorList.Add(ResourceKeyResolver.ErrorSemesterAlreadyContainsGroup);
                return response;
            }
            if (NinjectResolver.GetInstance<ISemesterSubjectService>().GetSemesterById(SemesterId) == null)
            {
                response.ErrorList.Add(ResourceKeyResolver.ErrorSemesterDoesntExist);
                return response;
            }

            IsValid = true;

            return response;
        }
    }
}
