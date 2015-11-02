using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.ViewModels.File;

namespace StudentNotes.Web.Controllers
{
    public class NoteController : Controller
    {
        // GET: Note
        public ActionResult PrivateNotes()
        {
            UserNotesViewModel viewModel = new UserNotesViewModel();
            viewModel.GetPrivateUserNotes((int)Session["CurrentUserId"]);


            return PartialView("~/Views/Partials/MyNotes/PrivateNotesPartial.cshtml", viewModel);
        }
    }
}