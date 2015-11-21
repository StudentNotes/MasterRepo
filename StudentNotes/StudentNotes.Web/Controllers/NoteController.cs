using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.File;
using StudentNotes.Web.Filters;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
    public class NoteController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IUploadService _uploadService;

        public NoteController(IFileService fileService, IUploadService uploadService)
        {
            _fileService = fileService;
            _uploadService = uploadService;
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
            UserNotesViewModel model = new UserNotesViewModel();
            var sharedFiles = _fileService.GetSharedUserFiles((int) Session["CurrentUserId"]);

            foreach (var sharedFile in sharedFiles)
            {
                model.UserNotesList.Add(new Note(sharedFile));
            }

            return PartialView("~/Views/Partials/MyNotes/SharedNotesPartial.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> DeletePrivateNote(int fileId)
        {
            var file = _fileService.GetFileById(fileId);
            string remoteFilePath = file.Path + "/" + file.Name;

            await _uploadService.DeletePrivateNoteAsync(fileId);

            return RedirectToAction("PrivateNotes");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteSemesterSubjectNote(int noteId, int semesterSubjectId)
        {
            var responseCode = await _uploadService.DeleteSemesterSubjectNoteAsync(noteId, semesterSubjectId);

            return RedirectToAction("ShowNotes", "University", new { semesterSubjectId });
        }

        [HttpPost]
        public ActionResult ShareNoteToGroup(int fileId, int groupId, int semesterSubjectId)
        {
            return null;
        }

        [HttpPost]
        public ActionResult ShareNoteToUser(int fileId, int userId = 0, string userEmail = "")
        {
            return null;
        }

        [HttpPost]
        public ActionResult RemoveShareToGroup(int fileId, int groupId, int semesterSubjectId)
        {
            return null;
        }

        [HttpPost]
        public ActionResult RemoveShareToUser(int fileId, int userId)
        {
            return null;
        }
    }
}