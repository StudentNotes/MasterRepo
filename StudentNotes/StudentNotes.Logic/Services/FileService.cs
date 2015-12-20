using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Logic.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly ISemesterSubjectFileRepository _semesterSubjectFileRepository;
        private readonly IFileSharedGroupRepository _fileSharedGroupRepository;
        private readonly IUserSharedFileRepository _userSharedFileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserPreferencesRepository _userPreferencesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FileService(IFileRepository fileRepository, ISemesterSubjectFileRepository semesterSubjectFileRepository, IFileSharedGroupRepository fileSharedGroupRepository,
            IUserSharedFileRepository userSharedFileRepository, IUserRepository userRepository, IUserPreferencesRepository userPreferencesRepository, IUnitOfWork unitOfWork)
        {
            _fileRepository = fileRepository;
            _semesterSubjectFileRepository = semesterSubjectFileRepository;
            _fileSharedGroupRepository = fileSharedGroupRepository;
            _userSharedFileRepository = userSharedFileRepository;
            _userRepository = userRepository;
            _userPreferencesRepository = userPreferencesRepository;
            _unitOfWork = unitOfWork;
        }


        public List<SecureUserModel> GetUsersWithFileAccess(int fileId)
        {
            var userIds = _userSharedFileRepository.GetMany(usf => usf.FileId == fileId).Select(file => file.UserId).ToList();
            var users = _userRepository.GetMany(u => userIds.Contains(u.UserId));
            var secureUsers = users.Select(user => new SecureUserModel(user)).ToList();

            return secureUsers;
        }

        public IEnumerable<File> GetPrivateFiles(int userId)
        {
            var allUserFiles = _fileRepository.GetMany(f => f.UserId == userId).ToList();
            var allUserFileIds = allUserFiles.Select(f => f.FileId).ToList();

            var semesterSubjectFiles = _semesterSubjectFileRepository.GetMany(ss => allUserFileIds.Contains(ss.FileId)).Select(f => f.FileId).ToList();

            allUserFiles.RemoveAll(file => semesterSubjectFiles.Contains(file.FileId));

            return allUserFiles;
        }

        public IEnumerable<File> GetFilesBySemesterSubjectId(int semesterSubjectId)
        {
            var fileIds =
                _semesterSubjectFileRepository.GetMany(ssf => ssf.SemesterSubjectId == semesterSubjectId)
                    .Select(f => f.FileId).ToList();
            var files = _fileRepository.GetMany(f => fileIds.Contains(f.FileId));
            return files;
        }

        public IEnumerable<File> GetSemesterSubjectFilesByUserId(int semesterSubjectId, int userId)
        {
            var semesterSubjectFileIds =
                _semesterSubjectFileRepository.GetMany(f => f.SemesterSubjectId == semesterSubjectId).Select(f => f.FileId);

            var userFiles = from f in _fileRepository.GetMany(sf => semesterSubjectFileIds.Contains(sf.FileId))
                where f.UserId == userId
                select f;

            return userFiles;
        }

        public IEnumerable<File> GetSharedUserFiles(int userId)
        {
            var sharedFiles = _fileRepository.GetMany(u => u.UserId == userId && u.IsShared);
            return sharedFiles;
        }

        public IEnumerable<File> GetRecentlyAddedFiles(int userId)
        {
            var lastUploadDays = _userPreferencesRepository.GetById(userId).LastUploadDays;
            if (lastUploadDays == null)
            {
                return null;
            }
            var daysInterval = (double)lastUploadDays;
            var oldDate = (DateTime.Now).AddDays(-daysInterval);

            var recentlyAddedFiles =
                _fileRepository.GetMany(f => f.UserId == userId && DateTime.Compare(f.UploadDate, oldDate) >= 0);

            return recentlyAddedFiles;
        }

        public List<File> GetSharedGroupFiles(int userId)
        {
            var userFiles = _fileRepository.GetMany(f => f.UserId == userId).ToList();
            var userFileIds = userFiles.Select(f => f.FileId).ToList();

            var sharedFilesToGroupIds =
                _fileSharedGroupRepository.GetMany(fsg => userFileIds.Contains(fsg.FileId))
                    .Select(f => f.FileId)
                    .ToList();

            userFiles.RemoveAll(file => !sharedFilesToGroupIds.Contains(file.FileId));

            return userFiles;
        }

        public File GetFileById(int fileId)
        {
            return _fileRepository.GetById(fileId);
        }

        public SecureUserModel GetSecureUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            return new SecureUserModel(user);
        }

        public bool IsPrivateFile(int fileId)
        {
            var file = _semesterSubjectFileRepository.GetMany(f => f.FileId == fileId);
            return file != null;
        }

        public bool UserHasAccess(int fileId, int userId)
        {
            var userSharedFile = _userSharedFileRepository.GetMany(usf => usf.FileId == fileId && usf.UserId == userId);
            return userSharedFile.Count() != 0;
        }

        public int AddFileToUser(int fileId, int userId)
        {
            
            try
            {
                _userSharedFileRepository.Add(new UserSharedFile()
                {
                    FileId = fileId,
                    UserId = userId
                });
                var fileToUpdate = _fileRepository.GetById(fileId);
                fileToUpdate.IsShared = true;
                _fileRepository.Update(fileToUpdate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
            _unitOfWork.Commit();
            return 0;
        }

        public int AddFileToUser(int fileId, string email)
        {
            var user = _userRepository.GetMany(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                return -1;
            }
            var userId = user.UserId;
            try
            {
                _userSharedFileRepository.Add(new UserSharedFile()
                {
                    FileId = fileId,
                    UserId = userId
                });
                var fileToUpdate = _fileRepository.GetById(fileId);
                fileToUpdate.IsShared = true;
                _fileRepository.Update(fileToUpdate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
            _unitOfWork.Commit();
            return 0;
        }

        public int RemoveFileFromUser(int fileId, int userId)
        {
            try
            {
                var entityToDelete = _userSharedFileRepository.Get(usf => usf.FileId == fileId && usf.UserId == userId);
                _userSharedFileRepository.Delete(entityToDelete);
                _userSharedFileRepository.Commit();

                var fileSharedUser = _userSharedFileRepository.GetMany(f => f.FileId == fileId);
                var fileSharedGroup = _fileSharedGroupRepository.GetMany(f => f.FileId == fileId);

                if (fileSharedUser.Any() || fileSharedGroup.Any())
                {
                    return 1;
                }

                var fileToUpdate = _fileRepository.GetById(fileId);
                fileToUpdate.IsShared = false;
                _fileRepository.Update(fileToUpdate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
            _unitOfWork.Commit();
            return 0;
        }

        public int RemoveFileFromUser(int fileId, string email)
        {
            var user = _userRepository.GetMany(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                return -1;
            }
            var userId = user.UserId;
            try
            {
                var entityToDelete = _userSharedFileRepository.GetMany(usf => usf.FileId == fileId && usf.UserId == userId).FirstOrDefault();
                _userSharedFileRepository.Delete(entityToDelete);

                var fileSharedUser = _userSharedFileRepository.GetMany(f => f.FileId == fileId);
                var fileSharedGroup = _fileSharedGroupRepository.GetMany(f => f.FileId == fileId);

                if (fileSharedUser.Any() || fileSharedGroup.Any()) return 0;

                var fileToUpdate = _fileRepository.GetById(fileId);
                fileToUpdate.IsShared = false;
                _fileRepository.Update(fileToUpdate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
            return 0;
        }

        public void SaveFile()
        {
            _unitOfWork.Commit();
        }
    }
}
