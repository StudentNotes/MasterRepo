﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.Services;
using StudentNotes.Logic.ViewModels.File;
using StudentNotes.Logic.ViewModels.Home;

namespace StudentNotes.Web.Controllers
{
    public class FileController : AsyncController
    {
        private readonly IUploadService _uploadService;
        private readonly IFileService _fileService;

        public FileController(IUploadService uploadService, IFileService fileService)
        {
            _uploadService = uploadService;
            _fileService = fileService;
        }
        // GET: File
        [HttpPost]
        public async Task<ActionResult> SendFile(IEnumerable<HttpPostedFileBase> files, string fileType, string uploadPath, string semesterSubjectId)
        {
            var anyFile = files.First();
            if (anyFile == null)
            {
                HomeViewModel model = new HomeViewModel();
                model.LoginViewModel.ErrorList.Add("No file selected", "Nie wybrałeś żadnego pliku do wysłania!");

                return View("~/Views/LoggedIn/Index.cshtml", model);
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
                    Size = fileContent.Length.ToString(),
                    Category = CategorySelector.GetCategory(fileName)
                    //Tags = fileTags
                };

                if (fileType == "Private")
                {
                    if (await _uploadService.UploadPrivateNote(note, (int)Session["CurrentUserId"]) == 0)
                    {
                        _uploadService.SaveUpload();
                    }
                }
                else if (fileType == "University")
                {
                    if (uploadPath.IsEmpty())
                    {
                        return RedirectToAction("RetriveInfo", new { errorCode = 1 });
                    }
                    if (await _uploadService.UploadUniversityNote(note, (int)Session["CurrentUserId"], uploadPath, int.Parse(semesterSubjectId)) == 0)
                    {
                        _uploadService.SaveUpload();
                    }
                }
                

            }
            return RedirectToAction("RetriveInfo", new{errorCode = 0});
        }


        [HttpGet]
        public  ActionResult RetriveInfo(int errorCode)
        {
            switch (errorCode)
            {
                case 0:
                {
                    HomeViewModel model = new HomeViewModel();
                    model.LoginViewModel.SuccessList.Add("UploadSuccess", WebResponseCode.FileUploadedSuccessfully);
                    return View("~/Views/LoggedIn/Index.cshtml", model);
                }
                case 1:
                {
                    HomeViewModel model = new HomeViewModel();
                    model.LoginViewModel.ErrorList.Add("NotAllDataPassed", WebResponseCode.WrongDataPassed);
                    return View("~/Views/LoggedIn/Index.cshtml", model);
                }
                    
            }
            return View("~/Views/Note/NoteUploaded.cshtml");
        }



        //[HttpGet]
        //public async Task<string> HeavyAction(int holdTime)
        //{
        //    Debug.WriteLine(GetMessage(holdTime));
        //    return "Now I'm done!";
        //}

        //private async Task<ActionResult> GetMessage(int holdTime)
        //{
        //    await Task.Delay(holdTime);
        //    return RedirectToAction("RetriveInfo", new { errorCode = 0 });
        //}
    }
}