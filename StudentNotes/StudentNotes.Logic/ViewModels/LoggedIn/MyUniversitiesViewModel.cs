using System.Collections.Generic;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.LoggedIn
{
    public class MyUniversitiesViewModel
    {
        public int UniversityId { get; set; } 
        public string UniversityName { get; set; }
        public List<Grade> Grades { get; set; }
        public List<StudySubject> StudySubjects { get; set; } 

        public MyUniversitiesViewModel()
        {
            Grades = new List<Grade>();
            StudySubjects = new List<StudySubject>();
        }
    }
}
