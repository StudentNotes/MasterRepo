using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.File;
using StudentNotes.Logic.ViewModels.ManageNotes;
using StudentNotes.Web.Filters;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
    public class NoteController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IUploadService _uploadService;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;

        public NoteController(IFileService fileService, IUploadService uploadService, IUserService userService, IGroupService groupService)
        {
            _fileService = fileService;
            _uploadService = uploadService;
            _userService = userService;
            _groupService = groupService;
        }

        // GET: Note
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
        public ActionResult ShareNoteToGroup(int fileId, int groupId, int semesterSubjectId)
        {
            if (_fileService.GetFileById(fileId) == null || !_groupService.GroupExists(groupId) || !_groupService.SemesterSubjectExists(semesterSubjectId))
            {
                //  Dopisac obsługę, gdy jakieś dane nie zostały przekazane
            }
            if (_groupService.FileSharedToGroup(fileId, groupId, semesterSubjectId))
            {
                
            }
            _groupService.AddFileToGroup(fileId, groupId, semesterSubjectId);
            _groupService.Commit();

            var semesterId = _groupService.GetSemesterBySemesterSubject(semesterSubjectId).SemesterId;

            return RedirectToAction("ShowGroupNotes", "Group", new {semesterSubjectId, semesterId, groupId});
        }

        [HttpPost]
        public ActionResult ShareNoteToUser(int fileId, int userId = 0, string userEmail = "")
        {
            if (_fileService.GetFileById(fileId) == null)
            {
                
            }
            if (_userService.UserExists(userEmail))
            {
                _fileService.AddFileToUser(fileId, userEmail);
                return RedirectToAction("SharedNotes");
            }
            if (_userService.UserExists(userId))
            {
                _fileService.AddFileToUser(fileId, userId);
                return RedirectToAction("SharedNotes");
            }
            return null;


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