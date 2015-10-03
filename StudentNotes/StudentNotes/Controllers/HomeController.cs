using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentNotes.Controllers
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
    }
}