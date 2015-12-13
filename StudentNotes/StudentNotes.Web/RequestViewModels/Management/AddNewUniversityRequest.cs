using System.Web.WebPages;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Web.Models.ResourcesFinderLogic;

namespace StudentNotes.Web.RequestViewModels.Management
{
    public class AddNewUniversityRequest : RequestModelBase
    {
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public string UniversityDescription { get; set; }

        public override ResponseViewModelBase Validate()
        {
            var response = new ResponseMessageViewModel();

            if (UniversityName.IsEmpty())
            {
                response.ErrorList.Add(ResourceKeyResolver.ErrorUniversityNameNotDefined);
                return response;
            }

            IsValid = true;

            return response;
        }
    }
}
