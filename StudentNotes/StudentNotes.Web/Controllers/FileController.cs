using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Authorization;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Web.Filters;
using StudentNotes.Web.Models.ResourcesFinderLogic;
using StudentNotes.Web.RequestViewModels;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class FileController : AsyncController
    {
        private readonly IFileServerService _uploadService;
        private readonly IFileService _fileService;

        public FileController(IFileServerService uploadService, IFileService fileService)
        {
            _uploadService = uploadService;
            _fileService = fileService;
        }
        // GET: File
        [HttpPost]
        public async Task<ActionResult> SendFile(SendFileRequest request)
        {
            var response = request.Validate();
            if (!request.IsValid)
            {
                TempData["ResponseViewModel"] = response;
                return RedirectToAction("RetriveInfo");
            }
                
            foreach (var file in request.Files)
            {
                var fileName = Path.GetFileName(file.FileName);
                BinaryReader fileReader = new BinaryReader(file.InputStream);
                byte[] fileContent = fileReader.ReadBytes((int)file.InputStream.Length);

                Note note = new Note()
                {
                    Name = fileName,
                    Content = fileContent,
                    Size = fileContent.Length.ToString(),
                    Category = CategorySelector.GetCategory(fileName),
                    Tags = request.FileTags
                };

                if (request.FileType == "Private")
                {
                    if (await _uploadService.UploadPrivateNote(note, (int)Session["CurrentUserId"]) == 0)
                    {
                        _uploadService.SaveUpload();
                    }
                }
                else if (request.FileType == "University")
                {
                    if (await _uploadService.UploadUniversityNote(note, (int)Session["CurrentUserId"], request.UploadPath, int.Parse(request.SemesterSubjectId)) == 0)
                    {
                        _uploadService.SaveUpload();
                    }
                }
            }
            response.AddSuccess(ResourceKeyResolver.SuccessNoteUploaded);
            TempData["ResponseViewModel"] = response;

            return RedirectToAction("RetriveInfo");
        }


        [HttpGet]
        public  ActionResult RetriveInfo()
        {
            HomeViewModel responseModel = new HomeViewModel();
            if (TempData["ResponseViewModel"] != null)
            {
                responseModel.LoginViewModel = (LoginViewModel) TempData["ResponseViewModel"];
            }

            return View("~/Views/LoggedIn/Processed.cshtml", responseModel);
        }



        [HttpGet]
        public async Task<string> HeavyAction(int holdTime)
        {
            Debug.WriteLine(await GetMessage(holdTime));
            return "Now I'm done!";
        }

        private async Task<ActionResult> GetMessage(int holdTime)
        {
            await Task.Delay(holdTime);
            return RedirectToAction("RetriveInfo", new { errorCode = 0 });
        }
    }
}