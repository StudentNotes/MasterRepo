using System.Linq;
using System.Web.Mvc;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Authorization;
using StudentNotes.Logic.ViewModels.Group;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Web.Filters;
using StudentNotes.Web.Models.ResourcesFinderLogic;
using StudentNotes.Web.RequestViewModels.Group;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly ISemesterSubjectService _semesterSubjectService;
        private readonly IUserService _userService;

        public GroupController(IGroupService groupService, ISemesterSubjectService semesterSubjectService, IUserService userService)
        {
            _groupService = groupService;
            _semesterSubjectService = semesterSubjectService;
            _userService = userService;
        }

        [HttpPost]
        public ActionResult CreateGroup(CreateGroupRequest request)
        {
            var response = request.Validate();
            if (!request.IsValid)
            {
                TempData["ResponseViewModel"] = response;
                return RedirectToAction("GroupCreatedRedirect");
            }

            var responseCode = _groupService.AddGroup(request.GroupName, request.GroupDescription, (int) Session["CurrentUserId"], request.SemesterId);
            if (responseCode != 0)
            {
                response.AddError(ResourceKeyResolver.ErrorCriticalFailure);
                TempData["ResponseViewModel"] = response;
                return RedirectToAction("GroupCreatedRedirect");
            }

            _groupService.Commit();

            response.AddSuccess(ResourceKeyResolver.SuccessGroupAdded);
            TempData["ResponseViewModel"] = response;

            return RedirectToAction("GroupCreatedRedirect");
        }

        [HttpGet]
        public ActionResult GroupCreatedRedirect()
        {
            HomeViewModel model = new HomeViewModel();
            var responseModel = TempData["ResponseViewModel"];
            if (responseModel != null)
            {
                model.LoginViewModel = (LoginViewModel)responseModel;
            }

            return View("~/Views/LoggedIn/Processed.cshtml", model);
        }

        [HttpGet]
        public ActionResult ShowGroupSemesters(ShowGroupSemestersRequest request)
        {
            GroupSemestersModel model = new GroupSemestersModel();
            model.SemesterList = _groupService.GetGroupSemesters(request.GroupId).ToList();
            model.GroupId = request.GroupId;
            model.GroupName = request.GroupName;

            //if (model.SemesterList.Count == 1)
            //{
            //    var SemesterId = model.SemesterList[0].SemesterId;
            //    var SemesterName = string.Format("Semestr {0}", model.SemesterList[0].SemesterNumber);
            //    return RedirectToAction("ShowSubjects", new { SemesterId, request.GroupId, SemesterName, request.GroupName });
            //}

            return PartialView("~/Views/Partials/MyGroups/GroupSemestersPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult ShowSubjects(ShowGroupSubjectsRequest request)
        {
            GroupSemesterSubjectsViewModel model = new GroupSemesterSubjectsViewModel
            {
                SemesterSubjectList = _semesterSubjectService.GetSemesterSubjects(request.SemesterId).ToList(),
                SemesterId = request.SemesterId,
                SemesterName = request.SemesterName,
                GroupId = request.GroupId,
                GroupName = request.GroupName
            };

            return PartialView("~/Views/Partials/MyGroups/GroupSemesterSubjectsPartial.cshtml", model);
        }

        
        [HttpGet]
        public ActionResult ShowGroupNotes(ShowGroupNotesRequest request)
        {
            var model = new GroupSubjectNotesViewModel();
            var groupNotes = _groupService.GetGroupSemesterSubjectFiles(request.GroupId, request.SemesterSubjectId).ToList();
            foreach (var file in groupNotes)
            {
                model.GroupNotes.Add(new Note(file));
                model.IsOwnerOrAdmin.Add(file.UserId == (int) Session["CurrentUserID"]);
            }
            var semesterSubject = _semesterSubjectService.GetSemesterSubjectById(request.SemesterSubjectId);
            var group = _groupService.GetGroupById(request.GroupId);
            var semester = _semesterSubjectService.GetSemesterById(request.SemesterId);

            if (group.AdminId == (int) Session["CurrentUserId"] || _userService.IsServiceAdmin((int) Session["CurrentUserId"]))
            {
                for (int i = 0; i < model.IsOwnerOrAdmin.Count; i++)
                {
                    model.IsOwnerOrAdmin[i] = true;
                }
            }

            model.SemesterId = semester.SemesterId;
            model.SemesterName = string.Format("Semestr {0}", semester.SemesterNumber);
            model.GroupId = group.GroupId;
            model.GroupName = group.Name;
            model.SemesterSubjectId = semesterSubject.SemesterSubjectId;
            model.SemesterSubjectName = semesterSubject.Name;

            return PartialView("~/Views/Partials/MyGroups/GroupSubjectNotesPartial.cshtml", model);
        }

        [HttpPost]
        public ActionResult DeleteGroupSemesterSubjectNote(DeleteGroupNoteRequest request)
        {
            _groupService.RemoveGroupFileFromSemester(request.NoteId, request.GroupId, request.SemesterSubjectId);

            return RedirectToAction("ShowGroupNotes", new {request.SemesterSubjectId, request.GroupId, request.SemesterId});
        }
    }
}