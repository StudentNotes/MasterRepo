using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.Authorization;
using StudentNotes.Web.Models.ResourcesFinderLogic;

namespace StudentNotes.Web.RequestViewModels
{
    public class SendFileRequest : RequestModelBase
    {
        public IEnumerable<HttpPostedFileBase> Files { get; set; } 
        public string FileType { get; set; }
        public string UploadPath { get; set; }
        public string SemesterSubjectId { get; set; }

        public override ResponseViewModelBase Validate()
        {
            LoginViewModel responseModel = new LoginViewModel();

            if (Files.FirstOrDefault() == null)
            {
                responseModel.ErrorList.Add(ResourceKeyResolver.ErrorFileNotChoosen);
                IsValid = false;
                return responseModel;
            }

            if (FileType == "University")
            {
                if (UploadPath.IsEmpty())
                {
                    responseModel.ErrorList.Add(ResourceKeyResolver.ErrorWrongDataPassed);
                    IsValid = false;
                    return responseModel;
                }
            }
            IsValid = true;
            return responseModel;
        }
    }
}
