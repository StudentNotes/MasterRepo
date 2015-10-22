using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.ViewModels.Authorization;

namespace StudentNotes.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Title = "StudentNotes - strona domowa";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(string email, string password)
        {
            if (email == "email" && password == "password")
            {
                return View("~/Views/LoggedIn/Index.cshtml");
            }
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string login, string email, string password, string confirmPassword, string termsOfUse)
        {
            return View("~/Views/Home/Index.cshtml");
        }
    }
}