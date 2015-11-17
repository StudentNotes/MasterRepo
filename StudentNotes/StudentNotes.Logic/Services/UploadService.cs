using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Base;
using StudentNotes.FileTransferManager.Consts;
using StudentNotes.FileTransferManager.FtpClient;
using StudentNotes.FileTransferManager.FtpClient.FileTypes;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.Repositories;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Logic.Services
{
    public class UploadService : IUploadService
    {
        private readonly FileServer _fileServer;
        private readonly FileServerUser _fileServerUser;
        private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;
        private readonly ISemesterSubjectFileRepository _semesterSubjectFileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UploadService(IUserRepository userRepository, IFileRepository fileRepository, ISemesterSubjectFileRepository semesterSubjectFileRepository, IUnitOfWork unitOfWork)
        {
            _fileServer = new FtpServer(LogicConstants.FtpServerAddress);
            _fileServerUser = new FtpUser(LogicConstants.FtpUserLogin, LogicConstants.FtpUserPassword, _fileServer);
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _semesterSubjectFileRepository = semesterSubjectFileRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> UploadPrivateNote(Note note, int userId)
        {
            var targetDirectory = string.Format("/FTP/{0}/{1}", GetFileServerRoot(userId), "Private");
            _fileServerUser.GoToOrCreatePath(targetDirectory);

            var uploadResult = await _fileServerUser.UploadFile(new CommonFile(note.Name, note.Content));
            if (uploadResult == 226)
            {
                string tagsWithSeparator = note.Tags.Aggregate("", (current, tag) => current + string.Format("{0};", tag));
                _fileRepository.Add(new File()
                {
                    Name = note.Name,
                    Category = note.Category,
                    Path = targetDirectory,
                    Size = note.Size,
                    UploadDate = DateTime.Now,
                    IsShared = false,
                    UserId = userId,
                    FileTags = tagsWithSeparator
                });
                _fileRepository.Commit();
                return 0;
            }
            return -1;
        }

        public async Task<int> UploadUniversityNote(Note note, int userId, string filePath, int semesterSubjectId)
        {
            var targetDirectory = string.Format("/FTP/{0}/{1}", GetFileServerRoot(userId), filePath);
            _fileServerUser.GoToOrCreatePath(targetDirectory);

            var uploadResult = await _fileServerUser.UploadFile(new CommonFile(note.Name, note.Content));
            if (uploadResult == 226)
            {
                string tagsWithSeparator = note.Tags.Aggregate("", (current, tag) => current + string.Format("{0};", tag));
                _fileRepository.Add(new File()
                {
                    Name = note.Name,
                    Category = note.Category,
                    Path = targetDirectory,
                    Size = note.Size,
                    UploadDate = DateTime.Now,
                    IsShared = false,
                    UserId = userId,
                    FileTags = tagsWithSeparator,
                    SemesterSubjectFile = new List<SemesterSubjectFile>()
                    {
                        new SemesterSubjectFile()
                        {
                            SemesterSubjectId = semesterSubjectId
                        }
                    }
                });

                _fileRepository.Commit();
                return 0;
            }
            return -1;
        }

        public async Task<int> DeletePrivateNoteAsync(int fileId)
        {
            var privateFile = _fileRepository.GetById(fileId);
            var  remoteFilePath = privateFile.Path + "/" + privateFile.Name;

            var responseCode = await _fileServerUser.DeleteFile(remoteFilePath);
            if (responseCode == (int)FtpResponseCode.FileDeleted)
            {
                _fileRepository.Delete(f => f.FileId == fileId);
                _unitOfWork.Commit();
            }

            return responseCode;
        }

        public async Task<int> DeleteSemesterSubjectNoteAsync(int fileId, int semesterSubjectId)
        {
            var semesterSubjectFile =
                _semesterSubjectFileRepository.GetMany(
                    sf => sf.FileId == fileId && sf.SemesterSubjectId == semesterSubjectId).FirstOrDefault();
            if (semesterSubjectFile == null)
            {
                return (int) FtpResponseCode.FileDoesntExist;
            }
            var file = _fileRepository.GetById(semesterSubjectFile.FileId);
            var remoteFilePath = file.Path + "/" + file.Name;
            var responseCode = await _fileServerUser.DeleteFile(remoteFilePath);

            if (responseCode != (int) FtpResponseCode.FileDeleted)
            {
                return (int) FtpResponseCode.GlobalError;
            }
            _fileRepository.Delete(file);
            _semesterSubjectFileRepository.Delete(semesterSubjectFile);
            _unitOfWork.Commit();
            return (int)FtpResponseCode.FileDeleted;
        }

        //public async Task<int> DeleteUniversityFileTask(int fileId, int semesterSubjectId)
        //{
            
        //}

        public void SaveUpload()
        {
            _fileRepository.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        private string GetFileServerRoot(int userId)
        {
            var user = _userRepository.GetById(userId);

            if (user == null)
            {
                return "User with selected id not found";
            }

            return user.Email;
        }
    }
}
