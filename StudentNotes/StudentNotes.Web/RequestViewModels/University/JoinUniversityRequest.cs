using System.Linq;
using System.Web.WebPages;
using Ninject.Web.Mvc;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.Services;
using StudentNotes.Logic.ViewModels.Authorization;
using StudentNotes.Web.Models;
using StudentNotes.Web.Models.ResourcesFinderLogic;

namespace StudentNotes.Web.RequestViewModels.University
{
    public class JoinUniversityRequest : RequestModelBase
    {
        public string UniversityName { get; set; }
        public string Year { get; set; }
        public string UniversitySubject { get; set; }

        public override ResponseViewModelBase Validate()
        {
            LoginViewModel response = new LoginViewModel();

            if (UniversityName.IsEmpty() || UniversitySubject.IsEmpty() || Year.IsEmpty())
            {
                IsValid = false;
                response.AddError(ResourceKeyResolver.ErrorWrongDataPassed);
                return response;
            }
            var university = NinjectResolver.GetInstance<ISchoolService>().GetSchoolByName(UniversityName);
            if (university == null)
            {
                IsValid = false;
                response.AddError(ResourceKeyResolver.ErrorUniversityDoesntExist);
                return response;
            }
            var grade = university.Grade.FirstOrDefault(g => g.Year == Year);
            if (grade == null)
            {
                IsValid = false;
                response.AddError(ResourceKeyResolver.ErrorUniversityGradeDoesntExist);
                return response;
            }
            var studySubject = grade.StudySubject.FirstOrDefault(ss => ss.Name == UniversitySubject);
            if (studySubject == null)
            {
                IsValid = false;
                response.AddError(ResourceKeyResolver.ErrorStudySubjectDoesntExist);
                return response;
            }

            return response;
        }
    }
}
