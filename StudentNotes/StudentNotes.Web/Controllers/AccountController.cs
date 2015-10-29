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
        // GET: Account
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

            StudentNotesUser serviceUser = new StudentNotesUser();
            serviceUser.SetModelName((int)Session["CurrentUserId"]);

            return View("~/Views/LoggedIn/Index.cshtml", serviceUser);
        }

        public ActionResult Logoff()
        {
            Session.Abandon();
            return View("~/Views/Home/Index.cshtml", new HomeViewModel());
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