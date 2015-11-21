using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using StudentNotes.Logic.LogicModels;
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
            var universities = schools.Select(school => new SearchJson()
            {
                FieldId = school.SchoolId, FieldName = school.Name
            }).ToList();

            return Json(universities, JsonRequestBehavior.AllowGet);
        } 
        
        [HttpGet]
        public JsonResult GetStudySubjectsJson(string universityId)
        {
            MyUniversitiesViewModel tmpModel = _schoolService.GetStudySubjectsBySchoolAndUserId(
                int.Parse(universityId), (int) Session["CurrentUserId"]);
            List<SearchJson> studySubjects = new List<SearchJson>();
            for (int i = 0; i < tmpModel.StudySubjects.Count; i++)
            {
                studySubjects.Add(new SearchJson()
                {
                    FieldId = tmpModel.StudySubjects[i].StudySubjectId,
                    FieldName = tmpModel.Grades[i].Year + " - "  + tmpModel.StudySubjects[i].Name
                });
            }

            return Json(studySubjects, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSemestersJson(string studySubjectId)
        {
            List<Semester> semesterList = _schoolService.GetSemestersByStudySubjectId(int.Parse(studySubjectId)).ToList();
            List<SearchJson> semesters = new List<SearchJson>();
            foreach (var semester in semesterList)
            {
                semesters.Add(new SearchJson()
                {
                    FieldId = semester.SemesterId,
                    FieldName = "Semestr " + semester.SemesterNumber.ToString()
                });
            }
            return Json(semesters, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSemesterSubjectsJson(string semesterId)
        {
            List<SemesterSubject> semesterSubjectList =
                _schoolService.GetSemesterSubjectsBySemesterId(int.Parse(semesterId)).OrderBy(g => g.Name).ToList();

            List<SearchJson> semesterSubjects = new List<SearchJson>();

            foreach (var semesterSubject in semesterSubjectList)
            {
                semesterSubjects.Add(new SearchJson()
                {
                    FieldId = semesterSubject.SemesterSubjectId,
                    FieldName = semesterSubject.Name
                });
            }
            return Json(semesterSubjects, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSemesterUsersJson(int semesterId)
        {
            var secureUsers = _schoolService.GetUsersBySemesterId(semesterId).ToList().OrderBy(u => u.LastName).ToList();
            var me = secureUsers.First(u => u.UserId == (int) Session["CurrentUserId"]);
            secureUsers.Remove(me);
            List<SearchJson> users = new List<SearchJson>();

            foreach (var secureUser in secureUsers)
            {
                string fieldName;
                if (secureUser.Name.IsEmpty() || secureUser.LastName.IsEmpty())
                {
                    fieldName = secureUser.Email;
                }
                else
                {
                    fieldName = string.Format("{0} {1}", secureUser.Name, secureUser.LastName);
                }

                users.Add(new SearchJson()
                {
                    FieldId = secureUser.UserId,
                    FieldName = fieldName
                });
            }
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGroupsJson()
        {
            var groups = _groupService.GetUserGroups((int) Session["CurrentUserId"]).ToList();
            List<SearchJson> resultList = groups.Select(@group => new SearchJson()
            {
                FieldId = @group.GroupId, 
                FieldName = @group.Name
            }).ToList();

            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGroupSemestersJson(int groupId)
        {
            var semesters = _groupService.GetGroupSemesters(groupId);
            List<SearchJson> resultList = semesters.Select(semester => new SearchJson()
            {
                FieldId = semester.SemesterId,
                FieldName = string.Format("Semestr {0}", semester.SemesterNumber)
            }).ToList();

            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGroupSemesterSubjectsJson(int semesterId)
        {
            var semesterSubjects = _schoolService.GetSemesterSubjectsBySemesterId(semesterId);
            List<SearchJson> resultList = semesterSubjects.Select(subject => new SearchJson()
            {
                FieldId = subject.SemesterSubjectId,
                FieldName = subject.Name
            }).ToList();

            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
    }
}