using System.Collections.Generic;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ViewModels.ManageNotes
{
    public class SharedNoteDetailsViewModel
    {
        public Note Note { get; set; }
        public List<SecureUserModel> SharedToUsersList { get; set; }
        public List<GroupBasics> SharedToGroupsList { get; set; }

        public SharedNoteDetailsViewModel()
        {
            SharedToGroupsList = new List<GroupBasics>();
            SharedToUsersList = new List<SecureUserModel>();
        }
    }
}
