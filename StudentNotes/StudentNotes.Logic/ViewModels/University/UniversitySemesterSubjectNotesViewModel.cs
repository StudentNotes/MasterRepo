using System.Collections.Generic;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ViewModels.University
{
    public class UniversitySemesterSubjectNotesViewModel
    {
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public int StudySubjectId { get; set; }
        public string StudySubjectName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int SemesterSubjectId { get; set; }
        public string SemesterSubjectName { get; set; }
        public List<Note> Notes { get; set; }

        public UniversitySemesterSubjectNotesViewModel()
        {
            Notes = new List<Note>();
        }
    }
}
