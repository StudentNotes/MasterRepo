using System;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.Models;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Web.Models.ResourcesFinderLogic;

namespace StudentNotes.Web.RequestViewModels.Management
{
    public class ModifySchoolGradeRequest : RequestModelBase
    {
        public string SchoolGrade { get; set; }
        public int UniversityId { get; set; }

        public override ResponseViewModelBase Validate()
        {
            var response = new ResponseMessageViewModel();

            if (NinjectResolver.GetInstance<ISchoolService>().SchoolContainsGrade(UniversityId, SchoolGrade))
            {
                response.ErrorList.Add(ResourceKeyResolver.ErrorUniversityContainsGrade);
                return response;
            }

            IsValid = true;
            return response;
        }
    }
}
