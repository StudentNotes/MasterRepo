using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Group
{
    public class GroupSemesterSubjectsViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public List<SemesterSubject> SemesterSubjectList { get; set; }

        public GroupSemesterSubjectsViewModel()
        {
            SemesterSubjectList = new List<SemesterSubject>();
        }
    }
}
