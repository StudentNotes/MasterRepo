using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.Authorization;
using StudentNotes.Logic.ViewModels.Authorization;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Logic.ViewModels.Validation;

namespace StudentNotes.Web.Controllers
{
    public class AccountController : Controller
    {
        // Akcja służąca do sprawnego dodawania administratorów serwisu StudentNotes (bardzo bezpieczna :] )
        //[HttpGet]
        //public string RegisterAdmin(string login, string password, bool isServiceAdmin)
        //{
        //    StudentNotesUser serviceAdmin = new StudentNotesUser(login, password, isServiceAdmin);
        //    if (serviceAdmin.UserExistsInDatabase())
        //    {
        //        return "UŻYTKOWNIK JUŻ ISTNIEJE W BAZIE DANYCH I NIE MOŻE MIEĆ ROLI ADMINISTRATORA SERWISU!";
        //    }
        //    serviceAdmin.SaveUserInDatabase();
        //    return string.Format("WITAMY {0}, JESTEŚ NOWYM ADMINISTRATOREM SERWISU STUDENTNOTES", login);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            HomeViewModel viewModelContainer = new HomeViewModel();
            viewModelContainer.RegisterViewModel = model;

            ValidateRegisterViewModel(model);
            if (model.ErrorList.Count == 0)
            {
                StudentNotesUser serviceUser = new StudentNotesUser(model.Email, model.Password);

                if (serviceUser.UserExistsInDatabase())
                {
                    model.ErrorList.Add("UserExistsInDatabase", "Użytkownik o podanym adresie E-mail jest już zarejestrowany w systemie.");

                    return View("~/Views/Home/Index.cshtml", viewModelContainer);
                }
                serviceUser.SaveUserInDatabase();

                Session.Add("CurrentUserId", serviceUser.GetStudentNotesUserId());
                //TempData.Add("serviceUser", serviceUser);

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

            StudentNotesUser serviceUser = new StudentNotesUser();
            serviceUser.SetModelName((int)Session["CurrentUserId"]);

            return View("~/Views/LoggedIn/Index.cshtml", serviceUser);
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
                //  Sprawdzamy dane logowania
                StudentNotesUser serviceUser = new StudentNotesUser(model.Email, model.Password);
                if (!serviceUser.UserExistsInDatabase())
                {
                    model.ErrorList.Add("NoUserInSystem", "W systemie nie ma użytkownika o podanym adresie E-mail.");
                    return View("~/Views/Home/Index.cshtml", viewModelContainer);
                }
                if (!serviceUser.IsServiceUser())
                {
                    model.ErrorList.Add("WrongPassword", "Podaj prawdziwe hasło dostępu do serwisu.");
                    return View("~/Views/Home/Index.cshtml", viewModelContainer);
                }
                Session.Add("CurrentUserId", serviceUser.GetStudentNotesUserId());

                //return View("~/Views/LoggedIn/Index.cshtml", serviceUser);
                return RedirectToAction("LoginRedirect");
            }
            //  Zwracamy błędy
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

        [ChildActionOnly]
        public ActionResult GetNavbarTopPartial()
        {
            StudentNotesUser user = new StudentNotesUser();
            user.SetModelName((int)Session["CurrentUserId"]);

            return PartialView("~/Views/Partials/NavbarTopPartial.cshtml", user);
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
                model.ErrorList.Add("Email", emailError);
            if (passwordError != null)
                model.ErrorList.Add("Password", passwordError);
        }
    }
}