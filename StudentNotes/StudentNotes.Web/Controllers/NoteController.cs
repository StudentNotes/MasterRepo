using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
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
        private readonly IFileServerService _uploadService;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly IUnitOfWork _unitOfWork;

        public NoteController(IFileService fileService, IFileServerService uploadService, IUserService userService, IGroupService groupService, IUnitOfWork unitOfWork)
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
                file.Path = GetNotePath(file.Path);
                model.Notes.Add(new Note(file));
            }

            return PartialView("~/Views/Partials/MyNotes/NewestNotesPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult PrivateNotes()
        {
            var model = new UserNotesViewModel();

            var privateFiles = _fileService.GetPrivateFiles((int) Session["CurrentUserId"]);

            foreach (var file in privateFiles)
            {
                file.Path = GetNotePath(file.Path);
                model.UserNotesList.Add(new Note(file));
            }

            return PartialView("~/Views/Partials/MyNotes/PrivateNotesPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult UniversityNotes()
        {
            var model = new UniversityNotesViewModel();
            var files = _fileService.GetUniversityFiles((int) Session["CurrentUserId"]).ToList();
            foreach (var file in files)
            {
                file.Path = GetNotePath(file.Path);
                model.Notes.Add(new Note(file));
            }

            return PartialView("~/Views/Partials/MyNotes/UniversityNotesPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult SharedNotes()
        {
            var model = new SharedNotesViewModel();

            var sharedFiles = _fileService.GetSharedUserFiles((int) Session["CurrentUserId"]).ToList();

            if (!sharedFiles.Any())
            {
                model.Response.AddError(ResourceKeyResolver.ErrorNoContentToDisplay);
                return PartialView("~/Views/Partials/MyNotes/SharedNotesPartial.cshtml", model);
            }

            foreach (var sharedFile in sharedFiles)
            {
                var type = _fileService.IsPrivateFile(sharedFile.FileId) ? NoteType.Private : NoteType.University;

                model.Notes.Add(new SimpleNoteModel()
                {
                    NoteId = sharedFile.FileId,
                    Name = sharedFile.Name,
                    Category = sharedFile.Category,
                    Type = type
                });
            }

            return PartialView("~/Views/Partials/MyNotes/SharedNotesPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult AvailableNotes()
        {
            var model = _fileService.GetAccessedFiles((int)Session["CurrentUserId"]);

            return PartialView("~/Views/Partials/MyNotes/AccessedNotesPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult AllNotes()
        {
            var model = new AllNotesViewModel();

            var allFiles = _fileService.GetAllFiles((int) Session["CurrentUserId"]);
            if (!allFiles.Any())
            {
                model.Response.ErrorList.Add(ResourceKeyResolver.ErrorNoContentToDisplay);
            }

            foreach (var file in allFiles)
            {
                model.Notes.Add(file);
            }

            return PartialView("~/Views/Partials/MyNotes/AllNotesPartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult AllNotesDetails(int noteId, int noteType)
        {
            var model = new AllNotesDetailsViewModel();

            var file = _fileService.GetFileById(noteId);
            switch (noteType)
            {
                case 0: //Owner
                {
                    model.NoteType = 0;
                    break;
                }
                case 1: //PrivateShare
                {
                    var noteOwner = _fileService.GetSecureUser(file.UserId);
                        model.NoteOwner = new SimpleUserModel()
                        {
                            UserId = noteOwner.UserId,
                            Name = noteOwner.Name,
                            LastName = noteOwner.LastName,
                            Email = noteOwner.Email
                        };
                    model.NoteType = 1;
                    break;
                }
                case 2: //Group
                {
                    var groups = _fileService.GetFileGroupShares(file.FileId, (int) Session["CurrentUserId"]).ToList();
                    foreach (var group in groups)
                    {
                        model.NoteGroup.Add(new SimpleGroupModel()
                        {
                            GroupId = group.GroupId,
                            Name = group.Name,
                            Description = group.Description
                        });
                    }
                    model.NoteType = 2;
                    break;
                }
            }
            model.Note = new Note(file);
            model.NoteTags.NoteId = noteId;
            model.NoteTags.Tags = file.FileTags;

            return PartialView("~/Views/Partials/MyNotes/AllNotesDetailsPartial.cshtml", model);
        }

        [HttpPost]
        public ActionResult DeletePrivateShareNote(int noteId)
        {
            _fileService.RemoveFileFromUser(noteId, (int) Session["CurrentUserId"]);

            return RedirectToAction("AllNotes");
        }

        [HttpPost]
        public ActionResult RemoveAccessedShare(int fileId)
        {
            _fileService.RemoveFileFromUser(fileId, (int) Session["CurrentUserId"]);
            return RedirectToAction("AvailableNotes");
        }

        [HttpGet]
        public FileContentResult DownloadNote(int fileId)
        {
            var note = _uploadService.DownloadNote(fileId);
            return File(note.Content, note.Category, note.Name);
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
                return PartialView("~/Views/Partials/Validation/ResponseMessagePartial.cshtml", response);
            }

            _groupService.AddFileToGroup(request.FileId, request.GroupId, request.SemesterSubjectId);
            _groupService.Commit();
            response.AddSuccess(ResourceKeyResolver.SuccessNoteSharedToGroup);

            return PartialView("~/Views/Partials/Validation/ResponseMessagePartial.cshtml", response);
        }

        [HttpPost]
        public ActionResult ShareMyNoteToUser(ShareMyNoteToUserRequest request)
        {
            ResponseMessageViewModel response;
            response = (ResponseMessageViewModel)request.Validate();
            if (!request.IsValid)
            {
                return PartialView("~/Views/Partials/Validation/ResponseMessagePartial.cshtml", response);
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

            return PartialView("~/Views/Partials/Validation/ResponseMessagePartial.cshtml", response);
        }

        [HttpPost]
        public ActionResult ShareUniversityNoteToGroup(ShareUniversityNoteToGroupRequest request)
        {
            ResponseMessageViewModel response;
            response = (ResponseMessageViewModel)request.Validate();
            if (!request.IsValid)
            {
                return PartialView("~/Views/Partials/Validation/ResponseMessagePartial.cshtml", response);
            }

            _groupService.AddFileToGroup(request.FileId, request.GroupId, request.SemesterSubjectId);
            _groupService.Commit();
            response.AddSuccess(ResourceKeyResolver.SuccessNoteSharedToGroup);

            return PartialView("~/Views/Partials/Validation/ResponseMessagePartial.cshtml", response);
        }

        [HttpPost]
        public ActionResult ShareUniversityNoteToUser(ShareUniversityNoteToUserRequest request)
        {
            ResponseMessageViewModel response;
            response = (ResponseMessageViewModel)request.Validate();
            if (!request.IsValid)
            {
                return PartialView("~/Views/Partials/Validation/ResponseMessagePartial.cshtml", response);
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

            return PartialView("~/Views/Partials/Validation/ResponseMessagePartial.cshtml", response);
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

        private string GetNotePath(string serverPath)
        {
            var universityNotePath = new StringBuilder("");
            var pathNodes = serverPath.Split('/');
            for (int i = 3; i < pathNodes.Length; i++)
            {
                universityNotePath.Append(string.Format("{0}/", pathNodes[i]));
            }
            return universityNotePath.ToString();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddTagToNote(int noteId, string tagName)
        {
            var model = new NoteTagsViewModel();
            model.NoteId = noteId;

            var isValid = true;

            if (tagName.IsEmpty())
            {
                model.Response.AddError(ResourceKeyResolver.ErrorNoTagChoosen);
                isValid = false;
            }
            if (!_fileService.TagExistsInDatabase(tagName))
            {
                model.Response.AddError(ResourceKeyResolver.ErrorTagDoesntExist);
                isValid = false;
            }
            if (_fileService.FileHasTag(noteId, tagName))
            {
                model.Response.AddError(ResourceKeyResolver.ErrorNoteHasTag);
                isValid = false;
            }

            if (!isValid)
            {
                model.Tags = _fileService.GetFileById(noteId).FileTags;
                return PartialView("~/Views/Partials/ManageNotes/NoteTagsPartial.cshtml", model);
            }

            _fileService.AddTagToFile(noteId, tagName);
            _unitOfWork.Commit();

            model.Tags = _fileService.GetFileById(noteId).FileTags;
            
            return PartialView("~/Views/Partials/ManageNotes/NoteTagsPartial.cshtml", model);
        }

        [HttpPost]
        public ActionResult RemoveTagFromNote(int noteId, string tagName)
        {
            var model = new NoteTagsViewModel {NoteId = noteId};
            var isValid = true;

            if (tagName.IsEmpty())
            {
                model.Response.AddError(ResourceKeyResolver.ErrorNoTagChoosen);
                isValid = false;
            }
            if (!_fileService.TagExistsInDatabase(tagName))
            {
                model.Response.AddError(ResourceKeyResolver.ErrorTagDoesntExist);
                isValid = false;
            }
            if (!_fileService.FileHasTag(noteId, tagName))
            {
                model.Response.AddError(ResourceKeyResolver.ErrorNoteHasNoTag);
                isValid = false;
            }

            if (!isValid)
            {
                model.Tags = _fileService.GetFileById(noteId).FileTags;
                return PartialView("~/Views/Partials/ManageNotes/NoteTagsPartial.cshtml", model);
            }

            _fileService.RemoveTagFromFile(noteId, tagName);
            _unitOfWork.Commit();

            model.Tags = _fileService.GetFileById(noteId).FileTags;

            return PartialView("~/Views/Partials/ManageNotes/NoteTagsPartial.cshtml", model);
        }
    }
}