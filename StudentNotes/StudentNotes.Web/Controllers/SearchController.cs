using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentNotes.Web.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        [HttpGet]
        public JsonResult UniversitySuggestions(string term)
        {
            var universitiesList = new List<string>()
            {
                "university_1",
                "university_2",
                "university_3",
                "university_4"
            };

            var filteredUniversities = universitiesList.Where(
                university => university.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
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
    }
}