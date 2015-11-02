using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.File;

namespace StudentNotes.Web.Controllers
{
    public class NoteController : Controller
    {
        private readonly IFileService _fileService;

        public NoteController(IFileService fileService)
        {
            _fileService = fileService;
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
    }
}