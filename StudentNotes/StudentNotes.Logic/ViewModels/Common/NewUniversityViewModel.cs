using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class NewUniversityViewModel
    {
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public string UniversityDescription { get; set; }

        //public List<string> GradeList { get; set; }
        public List<Grade> GradeList { get; set; } 
        public NewUniversityViewModel()
        {
            //GradeList = new List<string>();
            GradeList = new List<Grade>();
        }
    }
}
