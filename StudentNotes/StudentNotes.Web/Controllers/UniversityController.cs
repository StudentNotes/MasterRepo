using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Logic.ViewModels.LoggedIn;
using StudentNotes.Logic.ViewModels.University;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Web.Filters;

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
        public ActionResult JoinToUniversity(string universityName, string year, string universitySubject)
        {
            if (universitySubject.IsEmpty() || universityName.IsEmpty() || year.IsEmpty())
            {
                return RedirectToAction("StudySubjectJoinedRedirect", new {errorCode = (int)ErrorCode.WrongDataPassed});
            }
            var university = _schoolService.GetSchoolByName(universityName);
            if (university == null)
            {
                return RedirectToAction("StudySubjectJoinedRedirect", new { errorCode = (int)ErrorCode.UniversityDoesntExist });
            }
            var grade = university.Grade.FirstOrDefault(g => g.Year == year);
            if (grade == null)
            {
                return RedirectToAction("StudySubjectJoinedRedirect", new { errorCode = (int)ErrorCode.UniversityGradeDoesntExist });
            }
            var studySubject = grade.StudySubject.FirstOrDefault(ss => ss.Name == universitySubject);
            if (studySubject == null)
            {
                return RedirectToAction("StudySubjectJoinedRedirect", new { errorCode = (int)ErrorCode.StudySubjectDoesntExist });
            }

            if (!_schoolService.UserAddedToSchool((int) Session["CurrentUserId"], university.SchoolId))
            {
                _schoolService.AddUserToSchool((int)Session["CurrentUserId"], university.SchoolId);
            }

            if (_schoolService.UserJoinedStudySubject((int) Session["CurrentUserId"], studySubject.StudySubjectId))
            {
                return RedirectToAction("StudySubjectJoinedRedirect", new { errorCode = (int)ErrorCode.AllreadyJoinedToStudySubject });
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

            return RedirectToAction("StudySubjectJoinedRedirect", new { errorCode = 0 });
        }

        [HttpGet]
        public ActionResult StudySubjectJoinedRedirect(int errorCode)
        {
            HomeViewModel model = new HomeViewModel();
            switch (errorCode)
            {
                case 0:
                    model.LoginViewModel.SuccessList.Add("JoinedUniversity", WebResponseCode.JoinedStudySubject);
                    break;
                case (int)ErrorCode.UniversityDoesntExist:
                    model.LoginViewModel.ErrorList.Add("UniversityDoesntExist", WebResponseCode.UniversityDoesntExist);
                    break;
                case (int)ErrorCode.UniversityGradeDoesntExist:
                    model.LoginViewModel.ErrorList.Add("UniversityGradeDoesntExist", WebResponseCode.UniversityGradeDoesntExist);
                    break;
                case (int)ErrorCode.StudySubjectDoesntExist:
                    model.LoginViewModel.ErrorList.Add("StudySubjectDoesntExist", WebResponseCode.StudySubjectDoesntExist);
                    break;
                case (int)ErrorCode.WrongDataPassed:
                    model.LoginViewModel.ErrorList.Add("WrongDataPassed", WebResponseCode.WrongDataPassed);
                    break;
                case (int)ErrorCode.AllreadyJoinedToStudySubject:
                    model.LoginViewModel.ErrorList.Add("WrongDataPassed", WebResponseCode.AllreadyJoinedToStudySubject);
                    break;
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