using System;
using System.Text;

namespace StudentNotes.Logic.LogicModels
{
    public class GroupBasics
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int AdminId { get; set; }
        public string UniversityName { get; set; }
        public int UniversityId { get; set; }
        public string GradeName { get; set; }
        public int GradeId { get; set; }
        public string StudySubjectName { get; set; }
        public int StudySubjectId { get; set; }
        public string SemesterName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterSubjectName { get; set; }
        public int SemesterSubjectId { get; set; }

        public string GetSharePath()
        {
            var sb = new StringBuilder();
            sb.Append(string.Format("{0}/{1}/{2}/{3}/{4}", UniversityName, GradeName, StudySubjectName, SemesterName,
                SemesterSubjectName));

            return sb.ToString();
        }
    }


}
