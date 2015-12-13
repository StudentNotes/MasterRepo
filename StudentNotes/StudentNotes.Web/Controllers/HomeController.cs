using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.ViewModels.Authorization;
using StudentNotes.Logic.ViewModels.Home;

namespace StudentNotes.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeViewModel _viewModelContainer;

        public HomeController()
        {
            _viewModelContainer = new HomeViewModel();
        }
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Title = "StudentNotes - strona domowa";
            return View(_viewModelContainer);
        }
    }
}