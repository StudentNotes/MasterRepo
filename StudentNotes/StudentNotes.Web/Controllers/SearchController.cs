using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.File;
using StudentNotes.Web.Filters;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
    public class SearchController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly ISemesterSubjectService _studySubjectService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public SearchController(ISchoolService schoolService, ISemesterSubjectService studySubjectService,
            IFileService fileService, IUserService userService)
        {
            _schoolService = schoolService;
            _studySubjectService = studySubjectService;
            _fileService = fileService;
            _userService = userService;
        }

        // GET: Search
        [HttpGet]
        public JsonResult UniversitySuggestions(string term)
        {
            var universitiesList = _schoolService.GetSchoolsByTerm(term).ToList();
            var schoolNames = universitiesList.Select(university => university.Name).ToList();

            var filteredUniversities = schoolNames.Where(
                university => university.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

            return Json(filteredUniversities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GradeSuggestions(string universityNameGuess, string term)
        {
            List<string> gradeYears;

            if (universityNameGuess.IsEmpty())
            {
                gradeYears = _schoolService.GetAllGrades().Select(g => g.Year).ToList();
                List<string> tmpList = new List<string>();
                foreach (var year in gradeYears.Where(year => !tmpList.Contains(year)))
                {
                    tmpList.Add(year);
                }
                gradeYears = tmpList;
            }
            else
            {
                var university = _schoolService.GetSchoolByName(universityNameGuess);
                if (university == null)
                {
                    return null;
                }
                var gradesList = _schoolService.GetAllSchoolGrades(university.SchoolId);
                if (gradesList == null)
                {
                    return null;
                }

                gradeYears = gradesList.Select(g => g.Year).ToList();
            }
            
            var filteredGrades = gradeYears.Where(
                grade => grade.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

            return Json(filteredGrades, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult StudySubjectSuggestions(string universityNameGuess, string gradeYearGuess, string term)
        {
            var school = _schoolService.GetSchoolByName(universityNameGuess);
            if (school == null)
            {
                return null;
            }
            var grade = _schoolService.GetAllSchoolGrades(school.SchoolId).ToList().FirstOrDefault(g => g.Year == gradeYearGuess);
            if (grade == null)
            {
                return null;
            }
            List<string> studySubjects = grade.StudySubject.Select(ss => ss.Name).ToList();

            var filteredUniversities = studySubjects.Where(
                subject => subject.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

            return Json(filteredUniversities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SemesterSuggestions(string term)
        {
            var universitiesList = new List<string>()
            {
                "Semestr 1",
                "Semestr 2",
                "Semestr 3",
                "Semestr 4"
            };

            var filteredUniversities = universitiesList.Where(
                university => university.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

            return Json(filteredUniversities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SemesterSubjectsSuggestions(string term)
        {
            var subjects = _studySubjectService.GetAllSubjects().OrderBy(s => s.Name).Select(s => s.Name).ToList();

            if (subjects.Count == 0)
            {
                return null;
            }

            var filteredSubjects = subjects.Where(
                subject => subject.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

            return Json(filteredSubjects, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SubjectSuggestions(string term)
        {
            var subjectList = _studySubjectService.GetAllSubjects();
            var subjectNames = subjectList.Select(subject => subject.Name).ToList();

            var filteredSubjects = subjectNames.Where(
                subject => subject.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

            return Json(filteredSubjects, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TagSuggestions(string term)
        {
            var tags = _fileService.GetTagsStartingWith(term);

            return Json(tags, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FindNotesBy(string term)
        {
            var model = new SearchedNotesViewModel();

            var userId = (int) Session["CurrentUserId"];
            var userPreferences = _userService.GetUserPreferences(userId);

            switch (userPreferences.SearchMethod)
            {
                case "mixed":
                {
                    var files = _fileService.SearchFilesMixed(term, userId);
                    foreach (var file in files)
                    {
                        model.Notes.Add(new Note(file));
                    }
                    break;
                }
                case "tags":
                {
                    var files = _fileService.SearchFilesByTags(term, userId);
                    foreach (var file in files)
                    {
                        model.Notes.Add(new Note(file));
                    }
                    break;
                }
                case "names":
                {
                    var files = _fileService.SearchFilesByNames(term, userId);
                    foreach (var file in files)
                    {
                        model.Notes.Add(new Note(file));
                    }
                    break;
                }
            }
            return PartialView("~/Views/Partials/ManageNotes/SearchedNotesPartial.cshtml", model);
        }
    }
}