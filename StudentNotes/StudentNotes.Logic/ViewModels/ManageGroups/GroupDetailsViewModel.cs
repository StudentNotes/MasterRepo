using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;


namespace StudentNotes.Logic.ViewModels.ManageGroups
{
    public class GroupDetailsViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string CreatedOn { get; set; }
        public SecureUserModel Admin { get; set; }
    }
}
