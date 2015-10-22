using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.Authorization;
using StudentNotes.Logic.ViewModels.Authorization;
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
            ValidateRegisterViewModel(model);
            if (model.ErrorList.Count == 0)
            {
                StudentNotesUser serviceUser = new StudentNotesUser(model.Email, model.Password);

                serviceUser.UserExistsInDatabase();

                return View("~/Views/LoggedIn/Index.cshtml");
            }
            return View("~/Views/Home/Index.cshtml");
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

    }
}