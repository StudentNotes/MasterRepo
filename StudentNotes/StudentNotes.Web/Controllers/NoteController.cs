using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.File;
using StudentNotes.Logic.ViewModels.ManageNotes;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Web.Filters;
using StudentNotes.Web.Models.ResourcesFinderLogic;
using StudentNotes.Web.RequestViewModels.Note;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
    public class NoteController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IUploadService _uploadService;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly IUnitOfWork _unitOfWork;

        public NoteController(IFileService fileService, IUploadService uploadService, IUserService userService, IGroupService groupService, IUnitOfWork unitOfWork)
        {
            _fileService = fileService;
            _uploadService = uploadService;
            _userService = userService;
            _groupService = groupService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult RecentlyUploadedNotes()
        {
            var model = new RecentlyAddedNotesViewModel();
            if (TempData["ResponseViewModel"] != null)
            {
                var response = (ResponseMessageViewModel) TempData["ResponseViewModel"];
                model.Response = response;
            }

            var files = _fileService.GetRecentlyAddedFiles((int) Session["CurrentUserId"]).OrderByDescending(file => file.UploadDate);
            if (!files.Any())
            {
                model.Response.ErrorList.Add(ResourceKeyResolver.ErrorNoContentToDisplay);
            }
         
            foreach (var file in files)
            {
                model.Notes.Add(new Note(file));
            }

            return PartialView("~/Views/Partials/MyNotes/NewestNotesPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult PrivateNotes()
        {
            UserNotesViewModel model = new UserNotesViewModel();

            var privateFiles = _fileService.GetPrivateFiles((int) Session["CurrentUserId"]);

            foreach (var file in privateFiles)
            {
                model.UserNotesList.Add(new Note(file));
            }

            return PartialView("~/Views/Partials/MyNotes/PrivateNotesPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult SharedNotes()
        {
            var sharedFiles = _fileService.GetSharedUserFiles((int) Session["CurrentUserId"]);

            var model = (from sharedFile in sharedFiles
                let type = _fileService.IsPrivateFile(sharedFile.FileId) ? NoteType.Private : NoteType.University
                select new SharedNotesViewModel()
                {
                    NoteId = sharedFile.FileId,
                    Name = sharedFile.Name,
                    Category = sharedFile.Category,
                    Type = type
                }).ToList();

            return PartialView("~/Views/Partials/MyNotes/SharedNotesPartial.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteNote(int noteId)
        {
            await _uploadService.DeleteNote(noteId);

            return RedirectToAction("RecentlyUploadedNotes");
        }

        [HttpPost]
        public async Task<ActionResult> DeletePrivateNote(int fileId)
        {
            await _uploadService.DeletePrivateNoteAsync(fileId);

            return RedirectToAction("PrivateNotes");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteSemesterSubjectNote(int noteId, int semesterSubjectId)
        {
            await _uploadService.DeleteSemesterSubjectNoteAsync(noteId, semesterSubjectId);

            return RedirectToAction("ShowNotes", "University", new { semesterSubjectId });
        }

        [HttpPost]
        public ActionResult ShareMyNoteToGroup(ShareMyNoteToGroupRequest request)
        {
            ResponseMessageViewModel response;
            response = (ResponseMessageViewModel)request.Validate();
            if (!request.IsValid)
            {
                TempData["ResponseViewModel"] = response;
                return RedirectToAction("RecentlyUploadedNotes");
            }

            _groupService.AddFileToGroup(request.FileId, request.GroupId, request.SemesterSubjectId);
            _groupService.Commit();
            response.AddSuccess(ResourceKeyResolver.SuccessNoteSharedToGroup);

            TempData["ResponseViewModel"] = response;
            return RedirectToAction("RecentlyUploadedNotes");
        }

        [HttpPost]
        public ActionResult ShareMyNoteToUser(ShareMyNoteToUserRequest request)
        {
            ResponseMessageViewModel response;
            response = (ResponseMessageViewModel)request.Validate();
            if (!request.IsValid)
            {
                TempData["ResponseViewModel"] = response;
                return RedirectToAction("RecentlyUploadedNotes");
            }

            if (_userService.UserExists(request.UserName))
            {
                _fileService.AddFileToUser(request.FileId, request.UserName);
            }
            if (_userService.UserExists(request.UserId))
            {
                _fileService.AddFileToUser(request.FileId, request.UserId);
            }
            _unitOfWork.Commit();
            response.AddSuccess(ResourceKeyResolver.SuccessNoteSharedToUser);

            TempData["ResponseViewModel"] = response;
            return RedirectToAction("RecentlyUploadedNotes");
        }

        [HttpPost]
        public ActionResult ShareUniversityNoteToGroup(ShareUniversityNoteToGroupRequest request)
        {
            ResponseMessageViewModel response;
            response = (ResponseMessageViewModel)request.Validate();
            if (!request.IsValid)
            {
                TempData["ResponseViewModel"] = response;
                return RedirectToAction("ShowNotes", "University", new { semesterSubjectId = request.ReturnSemesterSubjectId });
            }

            _groupService.AddFileToGroup(request.FileId, request.GroupId, request.SemesterSubjectId);
            _groupService.Commit();
            response.AddSuccess(ResourceKeyResolver.SuccessNoteSharedToGroup);

            TempData["ResponseViewModel"] = response;
            return RedirectToAction("ShowNotes", "University", new { semesterSubjectId = request.ReturnSemesterSubjectId });
        }

        [HttpPost]
        public ActionResult ShareUniversityNoteToUser(ShareUniversityNoteToUserRequest request)
        {
            ResponseMessageViewModel response;
            response = (ResponseMessageViewModel)request.Validate();
            if (!request.IsValid)
            {
                TempData["ResponseViewModel"] = response;
                return RedirectToAction("ShowNotes", "University", new { semesterSubjectId = request.ReturnSemesterSubjectId });
            }

            if (_userService.UserExists(request.UserName))
            {
                _fileService.AddFileToUser(request.FileId, request.UserName);
            }
            if (_userService.UserExists(request.UserId))
            {
                _fileService.AddFileToUser(request.FileId, request.UserId);
            }
            _unitOfWork.Commit();
            response.AddSuccess(ResourceKeyResolver.SuccessNoteSharedToUser);

            TempData["ResponseViewModel"] = response;
            return RedirectToAction("ShowNotes", "University", new { semesterSubjectId = request.ReturnSemesterSubjectId });
        }

        [HttpPost]
        public ActionResult RemoveShareToGroup(int fileId, int groupId, int semesterSubjectId)
        {
            if (_groupService.RemoveFileFromGroup(fileId, groupId) == 0)
            {
                return RedirectToAction("SharedNotes");
            }
            return RedirectToAction("SharedNoteDetails", new { fileId });
        }

        [HttpPost]
        public ActionResult RemoveShareToUser(int fileId, int userId)
        {
            if (_fileService.RemoveFileFromUser(fileId, userId) == 0)
            {
                return RedirectToAction("SharedNotes");
            }
            return RedirectToAction("SharedNoteDetails", new {fileId});
        }

        [HttpGet]
        public ActionResult SharedNoteDetails(int fileId)
        {
            var model = new SharedNoteDetailsViewModel
            {
                Note = new Note(_fileService.GetFileById(fileId)),
                SharedToUsersList = _fileService.GetUsersWithFileAccess(fileId),
                SharedToGroupsList = _groupService.GetGroupsWithFileAccess(fileId)
            };

            return PartialView("~/Views/Partials/ManageNotes/SharedNoteDetailsPartial.cshtml", model);
        }
    }
}