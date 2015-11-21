using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.Logic.ViewModels.ManageGroups
{
    public class ManageGroupViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string StudySubject { get; set; }
        public string Semesters { get; set; }
        public int MemberNumber { get; set; }
    }
}
