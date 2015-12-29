using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class MyAccountViewModel
    {
        public UserPreferencesViewModel UserPreferences { get; set; }
        public SecureUserModel UserInfo { get; set; }
    }
}
