using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.FtpClient;
using StudentNotes.FileTransferManager.FtpClient.FileTypes;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.DBModels;

namespace StudentNotes.Logic.LogicModels.BrowserFile
{
    public class BrowserNoteUploader
    {
        public string TargetDirectory { get; private set; }
        public string FileServerRoot { get; private set; }
        private int _userId;

        public BrowserNoteUploader(string targetDirectory, int userId)
        {
            TargetDirectory = targetDirectory + "/";
            FileServerRoot = GetFileServerRoot(userId);
            _userId = userId;
        }

        public BrowserNoteUploader(int userId)
        {
            FileServerRoot = GetFileServerRoot(userId);
            TargetDirectory = "";
            _userId = userId;
        }

        public int UploadBrowserNote(Note note)
        {
            if (note.FileType == "Private")
            {
                TargetDirectory = "Private/";
                //FtpServer server = new FtpServer(LogicConstants.FtpServerAddress, LogicConstants.GetFtpServerFileTarget(FileServerRoot, TargetDirectory+note.Name));
                FtpServer server = new FtpServer(LogicConstants.FtpServerAddress);
                FtpUser user = new FtpUser(LogicConstants.FtpUserLogin, LogicConstants.FtpUserPassword, server);
                var destinationPath = string.Format("/FTP/{0}/{1}", FileServerRoot, TargetDirectory);
                user.GoToOrCreatePath(destinationPath);


                var uploadResult = user.UploadFile(new CommonFile(note.Name, note.Content));
                if (uploadResult == 226)
                {
                    using (var context = new StudentNotesContext())
                    {
                        try
                        {
                            context.File.Add(new File()
                            {
                                Name = note.Name,
                                Category = note.Category,
                                Path = destinationPath,
                                Size = note.Size,
                                UploadDate = DateTime.Now,
                                IsShared = false,
                                FileTags = "",
                                UserId = _userId
                            });

                            context.SaveChanges();

                            return 0;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error while updating the database...sorry");
                            return 1;
                        }
                        
                    }
                }
                else
                {
                    //  Failure
                    return 1;
                }

            }
            else
            {
                
            }
            
            return 0;
        }



        private string GetFileServerRoot(int userId)
        {
            using (var context = new StudentNotesContext())
            {
                var users = from u in context.User where u.UserId == userId select u;

                if (users.Any())
                {
                    return users.First().Email;
                }
                return "No user selected";
            }
        }
    }
}
