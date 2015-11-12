using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Logic.ViewModels.LoggedIn;

namespace StudentNotes.Web.Controllers
{
    public class LoggedInController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly IGroupService _groupService;

        public LoggedInController(ISchoolService schoolService, IGroupService groupService)
        {
            _schoolService = schoolService;
            _groupService = groupService;
        }
        // GET: LoggedIn
        public ActionResult GetNavbarSidePartial()
        {
            NavbarSideViewModel model = new NavbarSideViewModel();

            if (Session["CurrentUserId"] == null)
            {
                return View("~/Views/Home/Index.cshtml", new HomeViewModel());
            }

            var userGroups = _groupService.GetUserGroups((int) Session["CurrentUserId"]);
            var userSchools = _schoolService.GetByUser((int) Session["CurrentUserId"]);

            foreach (var userGroup in userGroups)
            {
                model.GroupList.Add(userGroup.GroupId, userGroup.Name);
            }
            foreach (var userSchool in userSchools)
            {
                model.UniversityList.Add(userSchool.SchoolId, userSchool.Name);
            }
            
            return PartialView("~/Views/Partials/NavbarSidePartial.cshtml", model);
        }
    }
}