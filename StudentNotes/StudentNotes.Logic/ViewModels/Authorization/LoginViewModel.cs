using StudentNotes.Logic.LogicAbstraction;

namespace StudentNotes.Logic.ViewModels.Authorization
{
    public class LoginViewModel : ResponseViewModelBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
