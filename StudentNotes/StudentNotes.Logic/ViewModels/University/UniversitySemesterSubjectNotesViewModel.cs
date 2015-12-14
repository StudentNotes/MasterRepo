using System.Collections.Generic;
using StudentNotes.Logic.LogicAbstraction;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Logic.ViewModels.University
{
    public class UniversitySemesterSubjectNotesViewModel
    {
        public ResponseViewModelBase Response { get; set; }
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
            Response = new ResponseMessageViewModel();
        }
    }
}
