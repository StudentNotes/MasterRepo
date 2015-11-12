using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class SemesterSubjectViewModel
    {
        public int UniversityId { get; set; }
        public List<Grade> GradeList { get; set; }

        public SemesterSubjectViewModel()
        {
            GradeList = new List<Grade>();
        }
    }
}
