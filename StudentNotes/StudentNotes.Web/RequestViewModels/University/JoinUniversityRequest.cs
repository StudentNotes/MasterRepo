using System.Linq;
using System.Web.WebPages;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.Models;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Authorization;
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
                response.AddError(ResourceKeyResolver.ErrorWrongDataPassed);
                return response;
            }
            var university = NinjectResolver.GetInstance<ISchoolService>().GetSchoolByName(UniversityName);
            if (university == null)
            {
                response.AddError(ResourceKeyResolver.ErrorUniversityDoesntExist);
                return response;
            }
            var grade = university.Grade.FirstOrDefault(g => g.Year == Year);
            if (grade == null)
            {
                response.AddError(ResourceKeyResolver.ErrorUniversityGradeDoesntExist);
                return response;
            }
            var studySubject = grade.StudySubject.FirstOrDefault(ss => ss.Name == UniversitySubject);
            if (studySubject == null)
            {
                response.AddError(ResourceKeyResolver.ErrorStudySubjectDoesntExist);
                return response;
            }

            IsValid = true;
            return response;
        }
    }
}
