using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.Models;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Web.Models.ResourcesFinderLogic;

namespace StudentNotes.Web.RequestViewModels.Note
{
    public class ShareUniversityNoteToGroupRequest : RequestModelBase
    {
        private readonly IFileService _fileService;
        private readonly IGroupService _groupService;
        
        public int ReturnSemesterSubjectId { get; set; }
        public int FileId { get; set; }
        public int GroupId { get; set; }
        public int SemesterSubjectId { get; set; }

        public ShareUniversityNoteToGroupRequest()
        {
            _fileService = NinjectResolver.GetInstance<IFileService>();
            _groupService = NinjectResolver.GetInstance<IGroupService>();
        }

        public override ResponseViewModelBase Validate()
        {
            var response = new ResponseMessageViewModel();

            if (_fileService.GetFileById(FileId) == null || !_groupService.GroupExists(GroupId) || !_groupService.SemesterSubjectExists(SemesterSubjectId))
            {
                response.ErrorList.Add(ResourceKeyResolver.ErrorWrongDataPassed);
                return response;
            }
            if (_groupService.FileSharedToGroup(FileId, GroupId, SemesterSubjectId))
            {
                response.ErrorList.Add(ResourceKeyResolver.ErrorGroupHasAccessToFile);
                return response;
            }

            IsValid = true;
            return response;
        }
    }
}
