using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class UniversityGradeStudySubjectViewModel
    {
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public List<Grade> StudyGrades { get; set; } 
    }
}
