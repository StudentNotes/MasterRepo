using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ViewModels.ManageGroups
{
    public class AddUserViewModel
    {
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public List<SecureUserModel> GroupUsers { get; set; }
        public List<SecureUserModel> SemesterUsers { get; set; }

        public AddUserViewModel()
        {
            GroupUsers = new List<SecureUserModel>();
            SemesterUsers = new List<SecureUserModel>();
        }
    }
}
