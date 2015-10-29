using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.ViewModels.Authorization;

namespace StudentNotes.Logic.ViewModels.Home
{
    public class HomeViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }

        public HomeViewModel()
        {
            LoginViewModel = new LoginViewModel();
            RegisterViewModel =new RegisterViewModel();
        }
    }
}
