using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Authorization;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Logic.ViewModels.LoggedIn;
using StudentNotes.Logic.ViewModels.University;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Web.Filters;
using StudentNotes.Web.Models.ResourcesFinderLogic;
using StudentNotes.Web.RequestViewModels.University;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
    public class UniversityController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly IFileService _fileService;

        public UniversityController(ISchoolService schoolService, IFileService fileService)
        {
            _schoolService = schoolService;
            _fileService = fileService;
        }

        // GET: University
        [HttpPost]
        public ActionResult JoinToUniversity(JoinUniversityRequest request)
        {
            var response = request.Validate();
            if (!request.IsValid)
            {
                TempData["ResponseViewModel"] = response;
                return RedirectToAction("StudySubjectJoinedRedirect");
            }

            var university = _schoolService.GetSchoolByName(request.UniversityName);
            var grade = university.Grade.FirstOrDefault(g => g.Year == request.Year);
            var studySubject = grade.StudySubject.FirstOrDefault(ss => ss.Name == request.UniversitySubject);

            if (!_schoolService.UserAddedToSchool((int) Session["CurrentUserId"], university.SchoolId))
            {
                _schoolService.AddUserToSchool((int)Session["CurrentUserId"], university.SchoolId);
            }

            if (_schoolService.UserJoinedStudySubject((int) Session["CurrentUserId"], studySubject.StudySubjectId))
            {
                response.AddError(ResourceKeyResolver.ErrorAllreadyJoinedToStudySubject);
                TempData["ResponseViewModel"] = response;
                return RedirectToAction("StudySubjectJoinedRedirect");
            }
            
            foreach (var semester in studySubject.Semester)
            {
                semester.SemesterUser.Add(new SemesterUser()
                {
                    SemesterId = semester.SemesterId,
                    UserId = (int)Session["CurrentUserId"]
                });
            }
            _schoolService.Commit();

            return RedirectToAction("StudySubjectJoinedRedirect");
        }

        [HttpGet]
        public ActionResult StudySubjectJoinedRedirect()
        {
            HomeViewModel model = new HomeViewModel();
            var responseModel = TempData["ResponseViewModel"];
            if (responseModel != null)
            {
                model.LoginViewModel = (LoginViewModel) responseModel;
            }
            return View("~/Views/LoggedIn/Index.cshtml", model);
        }

        [HttpGet]
        public ActionResult ShowStudySubjects(int universityId)
        {
            MyUniversitiesViewModel model = _schoolService.GetStudySubjectsBySchoolAndUserId(universityId,
                (int) Session["CurrentUserId"]);

            
            return PartialView("~/Views/Partials/MyUniversities/UniversitySubjectsPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult ShowSemesters(int studySubjectId)
        {
            List<Semester> semesters = _schoolService.GetSemestersByStudySubjectId(studySubjectId).ToList();
            return PartialView("~/Views/Partials/MyUniversities/UniversitySemestersPartial.cshtml", semesters);
        }

        [HttpGet]
        public ActionResult ShowSubjects(int semesterId)
        {
            List<SemesterSubject> semesterSubjects =
                _schoolService.GetSemesterSubjectsBySemesterId(semesterId).OrderBy(g => g.Name).ToList();

            return PartialView("~/Views/Partials/MyUniversities/UniversitySemesterSubjectsPartial.cshtml", semesterSubjects);
        }

        [HttpGet]
        public ActionResult ShowNotes(int semesterSubjectId)
        {
            SemesterSubjectNoteViewModel model = new SemesterSubjectNoteViewModel();

            var files = _fileService.GetSemesterSubjectFilesByUserId(semesterSubjectId, (int)Session["CurrentUserId"]).ToList();
            foreach (var file in files)
            {
                model.NoteList.Add(new Note(file));
            }
            model.SemesterSubjectId = semesterSubjectId;

            return PartialView("~/Views/Partials/MyUniversities/UniversitySemesterSubjectNotesPartial.cshtml", model);
        }
        
    }
}