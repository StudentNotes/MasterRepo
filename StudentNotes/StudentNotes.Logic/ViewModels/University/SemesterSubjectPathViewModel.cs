

namespace StudentNotes.Logic.ViewModels.University
{
    public class SemesterSubjectPathViewModel
    {
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }
        public int StudySubjectId { get; set; }
        public string StudySubjectName { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int SemesterSubjectId { get; set; }
        public string SemesterSubjectName { get; set; }

        public string ShortenGradeSubject()
        {
            return $"{StudySubjectName} {GradeName}";
        }
    }
}
