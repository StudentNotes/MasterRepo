using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentNotes.Web.Controllers
{
    public class ManagementController : Controller
    {
        [HttpGet]
        public ActionResult ManageUniversities()
        {

            return PartialView("~/Views/Partials/Management/ManageUniversities.cshtml");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveUniversitySettings(string university, string grade, string studySubject, string semester,
            string semesterSubjectList)
        {

            return PartialView("~/Views/Partials/Management/ManageUniversities.cshtml");
        }
    }

}