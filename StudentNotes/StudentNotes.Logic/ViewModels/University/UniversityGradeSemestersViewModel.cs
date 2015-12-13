using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.University
{
    public class UniversityGradeSemestersViewModel
    {
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public int StudySubjectId { get; set; }
        public string StudySubjectName { get; set; }
        public List<Semester> Semesters { get; set; }

        public UniversityGradeSemestersViewModel()
        {
            Semesters = new List<Semester>();
        }
    }
}
