using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Group
{
    public class GroupSemestersModel
    {
        public List<Semester> SemesterList { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public GroupSemestersModel()
        {
            SemesterList = new List<Semester>();
        }
    }
}
