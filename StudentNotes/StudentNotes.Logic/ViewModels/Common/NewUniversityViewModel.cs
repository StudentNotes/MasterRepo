using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.Models;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class NewUniversityViewModel
    {
        public ResponseViewModelBase Response { get; set; }

        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public string UniversityDescription { get; set; }

        public List<Grade> GradeList { get; set; } 

        public NewUniversityViewModel()
        {
            Response = new ResponseMessageViewModel();
            GradeList = new List<Grade>();
        }

        public void UpdateGradeList()
        {
            var university = NinjectResolver.GetInstance<ISchoolService>().GetSchoolById(UniversityId);

            foreach (var schoolGrade in university.Grade)
            {
                GradeList.Add(new Grade()
                {
                    Year = schoolGrade.Year
                });
            }
        }
    }
}
