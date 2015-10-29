using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.Logic.ViewModels.Authorization
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public Dictionary<string, string> ErrorList { get; set; }

        public LoginViewModel()
        {
            ErrorList = new Dictionary<string, string>();
        }
    }
}
