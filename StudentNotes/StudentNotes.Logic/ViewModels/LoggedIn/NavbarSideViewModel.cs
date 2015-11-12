using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.Logic.ViewModels.LoggedIn
{
    public class NavbarSideViewModel
    {
        public Dictionary<int, string> UniversityList { get; set; }
        public Dictionary<int, string> GroupList { get; set; }

        public NavbarSideViewModel()
        {
            UniversityList = new Dictionary<int, string>();
            GroupList = new Dictionary<int, string>();
        }
    }
}
