using System.Collections.Generic;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.University
{
    public class UniversitySemesterSubjectsViewModel
    {
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public int StudySubjectId { get; set; }
        public string StudySubjectName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public List<SemesterSubject> SemesterSubjects { get; set; }

        public UniversitySemesterSubjectsViewModel()
        {
            SemesterSubjects = new List<SemesterSubject>();
        }
    }
}
