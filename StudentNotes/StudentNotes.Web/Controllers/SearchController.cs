using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.ServiceInterfaces;

namespace StudentNotes.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISchoolService _schoolService;
        private readonly ISemesterSubjectService _studySubjectService;

        public SearchController(ISchoolService schoolService, ISemesterSubjectService studySubjectService)
        {
            _schoolService = schoolService;
            _studySubjectService = studySubjectService;
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

            List<string> gradeYears = gradesList.Select(g => g.Year).ToList();
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
            var universitiesList = new List<string>()
            {
                "Informatyka",
                "Analiza Matematyczna",
                "Algerbra Liniowa",
                "Informatyka 2"
            };

            var filteredUniversities = universitiesList.Where(
                university => university.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

            return Json(filteredUniversities, JsonRequestBehavior.AllowGet);
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
    }
}