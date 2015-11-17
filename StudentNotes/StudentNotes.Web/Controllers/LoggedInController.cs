using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Logic.ViewModels.JSON;
using StudentNotes.Logic.ViewModels.LoggedIn;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Web.Filters;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
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

        [HttpGet]
        public JsonResult GetSchoolsJson()
        {
            var schools = _schoolService.GetByUser((int) Session["CurrentUserId"]).ToList();
            var universities = schools.Select(school => new UniversityJson()
            {
                UniversityId = school.SchoolId, UniversityName = school.Name
            }).ToList();

            return Json(universities, JsonRequestBehavior.AllowGet);
        } 
        
        [HttpGet]
        public JsonResult GetStudySubjectsJson(string universityId)
        {
            MyUniversitiesViewModel tmpModel = _schoolService.GetStudySubjectsBySchoolAndUserId(
                int.Parse(universityId), (int) Session["CurrentUserId"]);
            List<StudySubjectJson> studySubjects = new List<StudySubjectJson>();
            for (int i = 0; i < tmpModel.StudySubjects.Count; i++)
            {
                studySubjects.Add(new StudySubjectJson()
                {
                    StudySubjectId = tmpModel.StudySubjects[i].StudySubjectId,
                    StudySubjectName = tmpModel.StudySubjects[i].Name,
                    Grade = tmpModel.Grades[i].Year
                });
            }

            return Json(studySubjects, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSemestersJson(string studySubjectId)
        {
            List<Semester> semesterList = _schoolService.GetSemestersByStudySubjectId(int.Parse(studySubjectId)).ToList();
            List<SemesterJson> semesters = new List<SemesterJson>();
            foreach (var semester in semesterList)
            {
                semesters.Add(new SemesterJson()
                {
                    SemesterId = semester.SemesterId,
                    SemesterNumber = semester.SemesterNumber.ToString()
                });
            }
            return Json(semesters, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSemesterSubjectsJson(string semesterId)
        {
            List<SemesterSubject> semesterSubjectList =
                _schoolService.GetSemesterSubjectsBySemesterId(int.Parse(semesterId)).OrderBy(g => g.Name).ToList();

            List<SemesterSubjectJson> semesterSubjects = new List<SemesterSubjectJson>();

            foreach (var semesterSubject in semesterSubjectList)
            {
                semesterSubjects.Add(new SemesterSubjectJson()
                {
                    SemesterSubjectId = semesterSubject.SemesterSubjectId,
                    SemesterSubjectName = semesterSubject.Name
                });
            }
            return Json(semesterSubjects, JsonRequestBehavior.AllowGet);
        } 
    }
}