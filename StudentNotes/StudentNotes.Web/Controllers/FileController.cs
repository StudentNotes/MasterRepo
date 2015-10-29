using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.LogicModels.BrowserFile;
using StudentNotes.Logic.ViewModels.File;

namespace StudentNotes.Web.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        [HttpPost]
        public ActionResult SendFile(IEnumerable<HttpPostedFileBase> files, string fileType)
        {
            if (files == null || !files.Any())
            {
                //  Obsłużyć przypadek, że użytkownik nie wybrał pliku...
            }
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file.FileName);
                BinaryReader fileReader = new BinaryReader(file.InputStream);
                byte[] fileContent = fileReader.ReadBytes((int)file.InputStream.Length);

                Note note = new Note()
                {
                    Name = fileName,
                    Content = fileContent,
                    FileType = fileType
                };

                var browserNoteUploader = new BrowserNoteUploader((int)Session["CurrentUserId"]);

                if (browserNoteUploader.UploadBrowserNote(note) == 0)
                {
                    //  Prawidłowo wysłano plik na serwer
                }
            }

            UserNotesViewModel viewModel = new UserNotesViewModel();
            viewModel.GetPrivateUserNotes((int)Session["CurrentUserId"]);

            //return PartialView("~/Views/Partials/MyNotes/PrivateNotesPartial.cshtml", viewModel);
            return View("~/Views/File/Index.cshtml", viewModel);
        }

    }
}