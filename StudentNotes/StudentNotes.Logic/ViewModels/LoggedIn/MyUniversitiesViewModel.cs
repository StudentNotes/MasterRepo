using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.LoggedIn
{
    public class MyUniversitiesViewModel
    {
        public List<Grade> Grades { get; set; }
        public List<StudySubject> StudySubjects { get; set; } 

        public MyUniversitiesViewModel()
        {
            Grades = new List<Grade>();
            StudySubjects = new List<StudySubject>();
        }
    }
}
