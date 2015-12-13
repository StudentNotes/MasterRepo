using System.Collections.Generic;

namespace StudentNotes.Logic.ViewModels.Authorization
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool TermsOfUse { get; set; }

        public Dictionary<string, string> ErrorList { get; set; }

        public RegisterViewModel()
        {
            ErrorList = new Dictionary<string, string>();
        }
    }
}
