using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Group;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Web.Filters;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly ISemesterSubjectService _semesterSubjectService;

        public GroupController(IGroupService groupService, ISemesterSubjectService semesterSubjectService)
        {
            _groupService = groupService;
            _semesterSubjectService = semesterSubjectService;
        }

        public ActionResult CreateGroup(string groupName, string groupDescription, int semesterId)
        {
            HomeViewModel model = new HomeViewModel();

            if (groupName.IsEmpty() || semesterId == null || semesterId == 0)
            {
                model.LoginViewModel.ErrorList.Add(WebResponseCode.GroupNameEmpty);
                return View("~/Views/LoggedIn/Index.cshtml", model);
            }
            if (_groupService.GroupInSemesterExists(groupName, semesterId))
            {               
                model.LoginViewModel.ErrorList.Add(WebResponseCode.SemesterAlreadyContainsGroup);
                return View("~/Views/LoggedIn/Index.cshtml", model);
            }
            var response = _groupService.AddGroup(groupName, groupDescription, (int) Session["CurrentUserId"], semesterId);
            if (response != 0)
            {
                model.LoginViewModel.ErrorList.Add(WebResponseCode.GlobalError);
                return View("~/Views/LoggedIn/Index.cshtml", model);
            }

            _groupService.Commit();
            model.LoginViewModel.SuccessList.Add(WebResponseCode.GroupAddedSuccessfully);
            return View("~/Views/LoggedIn/Index.cshtml", model);

        }

        [HttpGet]
        public ActionResult ShowGroupSemesters(int groupId, string groupName)
        {
            GroupSemestersModel model = new GroupSemestersModel();
            model.SemesterList = _groupService.GetGroupSemesters(groupId).ToList();
            model.GroupId = groupId;
            model.GroupName = groupName;

            //if (model.SemesterList.Count == 1)
            //{
            //    var semesterId = model.SemesterList[0].SemesterId;
            //    var semesterName = string.Format("Semestr {0}", model.SemesterList[0].SemesterNumber);
            //    return RedirectToAction("ShowSubjects", new{semesterId, groupId, semesterName, groupName});
            //}

            return PartialView("~/Views/Partials/MyGroups/GroupSemestersPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult ShowSubjects(int semesterId, int groupId, string semesterName, string groupName)
        {
            GroupSemesterSubjectsViewModel model = new GroupSemesterSubjectsViewModel
            {
                SemesterSubjectList = _semesterSubjectService.GetSemesterSubjects(semesterId).ToList(),
                SemesterId = semesterId,
                SemesterName = semesterName,
                GroupId = groupId,
                GroupName = groupName
            };

            return PartialView("~/Views/Partials/MyGroups/GroupSemesterSubjectsPartial.cshtml", model);
        }

        
        [HttpGet]
        public ActionResult ShowGroupNotes(int semesterSubjectId, int semesterId, int groupId)
        {
            GroupSubjectNotesViewModel model = new GroupSubjectNotesViewModel();
            var groupNotes = _groupService.GetGroupSemesterSubjectFiles(groupId, semesterSubjectId).ToList();
            foreach (var file in groupNotes)
            {
                model.GroupNotes.Add(new Note(file));
            }
            var semesterSubject = _semesterSubjectService.GetSemesterSubjectById(semesterSubjectId);
            var group = _groupService.GetGroupById(groupId);
            var semester = _semesterSubjectService.GetSemesterById(semesterId);

            model.SemesterId = semester.SemesterId;
            model.SemesterName = string.Format("Semestr {0}", semester.SemesterNumber);
            model.GroupId = group.GroupId;
            model.GroupName = group.Name;
            model.SemesterSubjectId = semesterSubject.SemesterSubjectId;
            model.SemesterSubjectName = semesterSubject.Name;

            return PartialView("~/Views/Partials/MyGroups/GroupSubjectNotesPartial.cshtml", model);
        }

        [HttpPost]
        public ActionResult DeleteGroupSemesterSubjectNote(int noteId, int groupId, int semesterSubjectId)
        {

            return RedirectToAction("ShowGroupNotes");
        }
    }
}