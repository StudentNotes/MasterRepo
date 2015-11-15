using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.FileTransferManager.Base;
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

        public int UploadPrivateNote(Note note, int userId)
        {
            var targetDirectory = string.Format("/FTP/{0}/{1}", GetFileServerRoot(userId), "Private");
            _fileServerUser.GoToOrCreatePath(targetDirectory);

            var uploadResult = _fileServerUser.UploadFile(new CommonFile(note.Name, note.Content));
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

        public int UploadUniversityNote(Note note, int userId, string filePath, int semesterSubjectId)
        {
            var targetDirectory = string.Format("/FTP/{0}/{1}", GetFileServerRoot(userId), filePath);
            _fileServerUser.GoToOrCreatePath(targetDirectory);

            var uploadResult = _fileServerUser.UploadFile(new CommonFile(note.Name, note.Content));
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
