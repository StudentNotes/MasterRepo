using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.LogicModels.BrowserFile;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.Services;
using StudentNotes.Logic.ViewModels.File;

namespace StudentNotes.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly IUploadService _uploadService;

        public FileController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }
        // GET: File
        [HttpPost]
        public ActionResult SendFile(IEnumerable<HttpPostedFileBase> files, string fileType)
        {
            var anyFile = files.First();
            if (anyFile == null)
            {
                //  Obsłużyć przypadek, że użytkownik nie wybrał pliku...
                return RedirectToAction("LoginRedirect", "Account");
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
                    Size = fileContent.Length.ToString()
                    //Category = file.category
                    //Tags = fileTags
                };


                if (_uploadService.UploadPrivateNote(note, (int) Session["CurrentUserId"]) == 0)
                {
                    _uploadService.SaveUpload();
                    //  Prawidłowo wysłano plik na serwer
                }


                //var browserNoteUploader = new BrowserNoteUploader((int)Session["CurrentUserId"]);

                //if (browserNoteUploader.UploadPrivateNote(note) == 0)
                //{
                //    //  Prawidłowo wysłano plik na serwer
                //}
            }

            //return PartialView("~/Views/Partials/MyNotes/PrivateNotesPartial.cshtml", viewModel);
            return RedirectToAction("RetriveInfo");
        }


        [HttpGet]
        public ActionResult RetriveInfo()
        {
            return View("~/Views/Note/NoteUploaded.cshtml");
        }

    }
}