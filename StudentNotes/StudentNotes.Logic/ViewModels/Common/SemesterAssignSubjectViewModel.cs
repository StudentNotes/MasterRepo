using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class SemesterAssignSubjectViewModel
    {
        public int UniversityId { get; set; }
        public int StudySubjectId { get; set; }
        public List<Semester> SemesterList { get; set; }

        public SemesterAssignSubjectViewModel()
        {
            SemesterList = new List<Semester>();
        }
    }
}
