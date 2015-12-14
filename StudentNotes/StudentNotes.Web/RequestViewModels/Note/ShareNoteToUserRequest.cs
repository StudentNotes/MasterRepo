using System.Web.WebPages;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.Models;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Web.Models.ResourcesFinderLogic;

namespace StudentNotes.Web.RequestViewModels.Note
{
    public class ShareNoteToUserRequest : RequestModelBase
    {
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public int ReturnSemesterSubjectId { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public ShareNoteToUserRequest()
        {
            _fileService = NinjectResolver.GetInstance<IFileService>();
            _userService = NinjectResolver.GetInstance<IUserService>();
        }
        public override ResponseViewModelBase Validate()
        {
            var response = new ResponseMessageViewModel();

            if (_fileService.GetFileById(FileId) == null)
            {
                response.AddError(ResourceKeyResolver.ErrorFileNotChoosen);
                return response;
            }
            if (FileId == 0 || (UserId == 0 && UserName.IsEmpty()))
            {
                response.AddError(ResourceKeyResolver.ErrorWrongDataPassed);
                return response;
            }

            var userId = UserId == 0 ? _userService.GetServiceUserId(UserName) : UserId;

            if (_fileService.UserHasAccess(FileId, userId))
            {
                response.AddError(ResourceKeyResolver.ErrorUserHasAccessToFile);
                return response;
            }

            IsValid = true;
            return response;
        }
    }
}
