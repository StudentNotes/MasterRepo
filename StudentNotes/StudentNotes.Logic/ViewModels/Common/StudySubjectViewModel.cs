using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class StudySubjectViewModel
    {
        public int UniversityId { get; set; }
        public string UniversityDescription { get; set; }
        public List<StudySubject> StudySubjects { get; set; }
        public int GradeId { get; set; }

        public StudySubjectViewModel()
        {
            StudySubjects = new List<StudySubject>();
        }
    }
}
