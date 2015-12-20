using System.Web.Mvc;
using System.Web.WebPages;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Authorization;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Logic.ViewModels.Common;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        // Akcja służąca do sprawnego dodawania administratorów serwisu StudentNotes (bardzo bezpieczna :] )
        [HttpGet]
        public string RegisterAdmin(string login, string password)
        {
            _userService.AddAdmin(login, password);

            return string.Format("WITAMY {0}, JESTEŚ NOWYM ADMINISTRATOREM SERWISU STUDENTNOTES", login);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            HomeViewModel viewModelContainer = new HomeViewModel();
            viewModelContainer.RegisterViewModel = model;

            ValidateRegisterViewModel(model);
            if (model.ErrorList.Count == 0)
            {
                
                if (_userService.UserExists(model.Email))
                {
                    model.ErrorList.Add("UserExistsInDatabase", "Użytkownik o podanym adresie E-mail jest już zarejestrowany w systemie.");
                    return View("~/Views/Home/Index.cshtml", viewModelContainer);
                }

                _userService.AddUser(model.Email, model.Password);
                Session.Add("CurrentUserId", _userService.GetServiceUserId(model.Email));

                return RedirectToAction("RegisterRedirect");
            }
            return View("~/Views/Home/Index.cshtml", viewModelContainer);
        }

        public ActionResult RegisterRedirect()
        {
            if (Session["CurrentUserId"] == null)
            {
                return View("~/Views/Home/Index.cshtml", new HomeViewModel());
            }

            return View("~/Views/LoggedIn/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            HomeViewModel viewModelContainer = new HomeViewModel();
            viewModelContainer.LoginViewModel = model;

            ValidateLoginViewModel(model);
            if (model.ErrorList.Count == 0)
            {
                if (!_userService.UserExists(model.Email))
                {          
                    model.ErrorList.Add("W systemie nie ma użytkownika o podanym adresie E-mail.");
                    return View("~/Views/Home/Index.cshtml", viewModelContainer);
                }
                
                if (!_userService.UserAuthorized(model.Email, model.Password))
                {
                    model.ErrorList.Add("Podaj prawdziwe hasło dostępu do serwisu.");
                    return View("~/Views/Home/Index.cshtml", viewModelContainer);
                }
                Session.Add("CurrentUserId", _userService.GetServiceUserId(model.Email));

                return RedirectToAction("LoginRedirect");
            }
            return View("~/Views/Home/Index.cshtml", viewModelContainer);
        }
        public ActionResult LoginRedirect()
        {
            if (Session["CurrentUserId"] == null)
            {
                return View("~/Views/Home/Index.cshtml", new HomeViewModel());
            }

            return View("~/Views/LoggedIn/Index.cshtml");
        }

        public ActionResult Logoff()
        {
            Session.Abandon();
            return View("~/Views/Home/Index.cshtml", new HomeViewModel());
        }

        public ActionResult SessionExpired()
        {
            HomeViewModel viewModel = new HomeViewModel();
            viewModel.LoginViewModel.ErrorList.Add(WebResponseCode.SessionExpired);
            return View("~/Views/Home/Index.cshtml", viewModel);
        }

        [ChildActionOnly]
        public ActionResult GetNavbarTopPartial()
        {
            if (Session["CurrentUserId"] == null)
            {
                return View("~/Views/Home/Index.cshtml", new HomeViewModel());
            }
            UserViewModel model = new UserViewModel();
            UserInfo userInfo = _userService.GetAllServiceUserInfo((int) Session["CurrentUserId"]);

            if (userInfo.Name.IsEmpty())
            {
                model.Name = "Nieznajomy";
            }
            else
            {
                model.Name = userInfo.Name;
            }

            if (userInfo.LastName.IsEmpty())
            {
                model.LastName = "";
            }
            else
            {
                model.LastName = userInfo.LastName;
            }

            model.IsServiceAdmin = _userService.IsServiceAdmin((int) Session["CurrentUserId"]);

            return PartialView("~/Views/Partials/NavbarTopPartial.cshtml", model);
        }

        private void ValidateRegisterViewModel(RegisterViewModel model)
        {
            var emailError = FormInputValidator.ValidateEmail(model.Email);
            var passwordError = FormInputValidator.ValidatePassword(model.Password);
            var confirmPasswordError = FormInputValidator.ValidatePasswordConfirmation(model.Password, model.ConfirmPassword);

            if(emailError != null)
                model.ErrorList.Add("Email", emailError);
            if(passwordError != null)
                model.ErrorList.Add("Password", passwordError);
            if(confirmPasswordError != null)
                model.ErrorList.Add("ConfirmPassword", confirmPasswordError);
            if (model.TermsOfUse == false)
            {
                model.ErrorList.Add("TermsOfUse", "Nie zgodziłeś się na warunki regulaminowe");
            }
        }

        private void ValidateLoginViewModel(LoginViewModel model)
        {
            var emailError = FormInputValidator.ValidateEmail(model.Email);
            var passwordError = FormInputValidator.ValidatePassword(model.Password);

            if (emailError != null)
                model.ErrorList.Add(emailError);
            if (passwordError != null)
                model.ErrorList.Add(passwordError);
        }
    }
}