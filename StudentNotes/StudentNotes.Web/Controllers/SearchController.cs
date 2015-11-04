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
        private readonly IStudySubjectService _studySubjectService;

        public SearchController(ISchoolService schoolService, IStudySubjectService studySubjectService)
        {
            _schoolService = schoolService;
            _studySubjectService = studySubjectService;
        }

        // GET: Search
        [HttpGet]
        public JsonResult UniversitySuggestions(string term)
        {
            //var universitiesList = new List<string>()
            //{
            //    "university_1",
            //    "university_2",
            //    "university_3",
            //    "university_4"
            //};

            var universitiesList = _schoolService.GetAllSchhools();


            var filteredUniversities = universitiesList.Where(
                university => university.Name.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

            return Json(filteredUniversities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GradeSuggestions(string term)
        {
            var universitiesList = new List<string>()
            {
                "2012/2016",
                "2013/2017",
                "2014/2018",
                "2014/2019"
            };

            var filteredUniversities = universitiesList.Where(
                university => university.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

            return Json(filteredUniversities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult StudySubjectSuggestions(string term)
        {
            var universitiesList = new List<string>()
            {
                "Informatyka",
                "Budownictwo",
                "Medycyna",
                "Mechatronika"
            };

            var filteredUniversities = universitiesList.Where(
                university => university.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
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