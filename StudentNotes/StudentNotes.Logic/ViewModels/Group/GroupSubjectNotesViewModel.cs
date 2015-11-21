using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ViewModels.Group
{
    public class GroupSubjectNotesViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int SemesterSubjectId { get; set; }
        public string SemesterSubjectName { get; set; }
        public List<Note> GroupNotes { get; set; }

        public GroupSubjectNotesViewModel()
        {
            GroupNotes = new List<Note>();
        }
    }
}
