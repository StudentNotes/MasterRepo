using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Common;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Web.Controllers
{
    public class ManagementController : Controller
    {
        private readonly IStudySubjectService _studySubjectService;

        public ManagementController(IStudySubjectService studySubjectService)
        {
            _studySubjectService = studySubjectService;
        }

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

        [HttpGet]
        public ActionResult ManageSubjectsList()
        {
            var subjectList = _studySubjectService.GetAllSubjects().OrderBy(s => s.Name).ToList();
            SubjectViewModel model = new SubjectViewModel(subjectList);

            return PartialView("~/Views/Partials/Management/ManageSubjects.cshtml", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddSubject(string subjectName)
        {
            if (subjectName != "")
            {
                _studySubjectService.AddAndSaveSubject(subjectName);
            }

            var subjectList = _studySubjectService.GetAllSubjects().OrderBy(s => s.Name).ToList();
            SubjectViewModel model = new SubjectViewModel(subjectList);

            return PartialView("~/Views/Partials/Management/ManageSubjects.cshtml", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemoveSubjects(SubjectViewModel model)
        {
            foreach (var subject in model.Subjects)
            {
                if (subject.Value == true)
                {
                    _studySubjectService.DeleteSubjectAndSave(subject.Key);
                }
            }
            model = new SubjectViewModel(_studySubjectService.GetAllSubjects().OrderBy(s => s.Name).ToList());   
   
            return PartialView("~/Views/Partials/Management/ManageSubjects.cshtml", model);
        }

        [HttpPost]
        public ActionResult RemoveSubject(string subjectName)
        {
            if (!subjectName.IsEmpty())
            {
                _studySubjectService.DeleteSubjectAndSave(subjectName);
            }

            SubjectViewModel model = new SubjectViewModel(_studySubjectService.GetAllSubjects().OrderBy(s => s.Name).ToList());

            return PartialView("~/Views/Partials/Management/ManageSubjects.cshtml", model);
        }
    }

}