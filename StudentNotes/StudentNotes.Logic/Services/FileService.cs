using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.File;
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
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IFileTagPatternRepository _fileTagPatternRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FileService(IFileRepository fileRepository, ISemesterSubjectFileRepository semesterSubjectFileRepository, IFileSharedGroupRepository fileSharedGroupRepository,
            IUserSharedFileRepository userSharedFileRepository, IUserRepository userRepository, IUserPreferencesRepository userPreferencesRepository,
            IGroupUserRepository groupUserRepository, IGroupRepository groupRepository, 
            IFileTagPatternRepository fileTagPatternRepository, IUnitOfWork unitOfWork)
        {
            _fileRepository = fileRepository;
            _semesterSubjectFileRepository = semesterSubjectFileRepository;
            _fileSharedGroupRepository = fileSharedGroupRepository;
            _userSharedFileRepository = userSharedFileRepository;
            _userRepository = userRepository;
            _userPreferencesRepository = userPreferencesRepository;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _fileTagPatternRepository = fileTagPatternRepository;
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

        public IEnumerable<File> GetUniversityFiles(int userId)
        {
            var allUserFiles = _fileRepository.GetMany(f => f.UserId == userId).ToList();
            var allUserFileIds = allUserFiles.Select(f => f.FileId).ToList();

            var semesterSubjectFiles = _semesterSubjectFileRepository.GetMany(ss => allUserFileIds.Contains(ss.FileId)).Select(f => f.FileId).ToList();

            allUserFiles.RemoveAll(file => !semesterSubjectFiles.Contains(file.FileId));

            return allUserFiles;
        }

        public List<File> SearchFilesByNames(string term, int userId)
        {
            var resultList = new List<File>();

            var namesList = term.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();
            for(int i = 0; i < namesList.Count; i++)
            {
                namesList[i] = namesList[i].Replace(" ", "");
            }

            var accessibleFileIds = GetAccessedFileIds(userId);
            var allAccessibleFiles = _fileRepository.GetMany(f => accessibleFileIds.Contains(f.FileId)).ToList();

            foreach (var accessibleFile in allAccessibleFiles)
            {
                if (namesList.Any(n => accessibleFile.Name.StartsWith(n)))
                {
                    resultList.Add(accessibleFile);
                }
            }

            return resultList;
        }

        public List<File> SearchFilesByTags(string term, int userId)
        {
            var accessibleFileIds = GetAccessedFileIds(userId);
            var possibleHits = _fileRepository.GetByTag(term);

            possibleHits.RemoveAll(f => !accessibleFileIds.Contains(f.FileId));

            return possibleHits;
        }

        public List<File> SearchFilesMixed(string term, int userId)
        {
            var resultList = new List<File>();

            var namesList = term.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < namesList.Count; i++)
            {
                namesList[i] = namesList[i].Replace(" ", "");
            }

            var accessibleFileIds = GetAccessedFileIds(userId);
            var possibleHitsByTag = _fileRepository.GetByTag(term);
            possibleHitsByTag.RemoveAll(f => !accessibleFileIds.Contains(f.FileId));

            var allAccessibleFiles = _fileRepository.GetMany(f => accessibleFileIds.Contains(f.FileId));
            foreach (var accessibleFile in allAccessibleFiles)
            {
                if (namesList.Any(n => accessibleFile.Name.StartsWith(n)))
                {
                    resultList.Add(accessibleFile);
                }
            }

            resultList = resultList.Union(possibleHitsByTag).ToList();

            return resultList;
        }

        public IEnumerable<Group> GetFileGroupShares(int fileId, int memberId)
        {
            var fileGroups = _fileSharedGroupRepository.GetMany(fsg => fsg.FileId == fileId).Select(g => g.GroupId);
            var userGroups = _groupUserRepository.GetMany(gu => gu.UserId == memberId).Select(g => g.GroupId);
            var ownGroups = _groupRepository.GetMany(g => g.AdminId == memberId).Select(g => g.GroupId);

            var groups = _groupRepository.GetMany(g => (fileGroups.Contains(g.GroupId) && userGroups.Contains(g.GroupId)) || (fileGroups.Contains(g.GroupId) && ownGroups.Contains(g.GroupId)));

            return groups;
        }

        public List<string> GetTagsStartingWith(string term)
        {
            var tags = _fileTagPatternRepository.GetMany(ftp => ftp.Name.StartsWith(term));

            return tags.Select(t => t.Name).ToList();
        }

        public User GetPrivateShareUser(int fileId)
        {
            var userId = _fileRepository.GetById(fileId).UserId;
            var user = _userRepository.GetById(userId);

            return user;
        }

        public List<Note> GetAllFiles(int userId)
        {
            var accessedUserFileIds =
                _userSharedFileRepository.GetMany(usf => usf.UserId == userId).Select(f => f.FileId).ToList();
            var userGroupIds = _groupUserRepository.GetMany(gu => gu.UserId == userId).Select(g => g.GroupId).ToList();
            userGroupIds.AddRange(_groupRepository.GetMany(g => g.AdminId == userId).Select(g => g.GroupId).ToList());
            var accessedGroupFileIds =
                _fileSharedGroupRepository.GetMany(fsg => userGroupIds.Contains(fsg.GroupId))
                    .Select(f => f.FileId)
                    .ToList();

            var userFiles = _fileRepository.GetMany(f => f.UserId == userId).ToList();
            var accessedGroupFiles = _fileRepository.GetMany(f => accessedGroupFileIds.Contains(f.FileId)
                                        && f.UserId != userId).ToList();
            var accessedUserFiles = _fileRepository.GetMany(f => accessedUserFileIds.Contains(f.FileId)
                                        && f.UserId != userId).ToList();

            var noteList = userFiles.Select(userFile => new Note(userFile) {AccessThrough = NoteAccess.Owner}).ToList();
            noteList.AddRange(accessedGroupFiles.Select(groupFile => new Note(groupFile) {AccessThrough = NoteAccess.Group}));
            noteList.AddRange(accessedUserFiles.Select(userFile => new Note(userFile) {AccessThrough = NoteAccess.PrivateShare}));

            //var allFiles =
            //    _fileRepository.GetMany(
            //        f => accessedUserFileIds.Contains(f.FileId) || accessedGroupFileIds.Contains(f.FileId) ||
            //            f.UserId == userId);

            return noteList;
        }

        private List<int> GetAccessedFileIds(int userId)
        {
            var accessedUserFileIds =
                _userSharedFileRepository.GetMany(usf => usf.UserId == userId).Select(f => f.FileId).ToList();
            var userGroupIds = _groupUserRepository.GetMany(gu => gu.UserId == userId).Select(g => g.GroupId).ToList();
            userGroupIds.AddRange(_groupRepository.GetMany(g => g.AdminId == userId).Select(g => g.GroupId).ToList());
            var accessedGroupFileIds =
                _fileSharedGroupRepository.GetMany(fsg => userGroupIds.Contains(fsg.GroupId))
                    .Select(f => f.FileId)
                    .ToList();
            var userFileIds = _fileRepository.GetMany(f => f.UserId == userId).Select(f => f.FileId).ToList();

            var fileIds = userFileIds.Union(accessedGroupFileIds).ToList().Union(accessedUserFileIds).ToList();

            return fileIds;
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

        public AccessedNotesViewModel GetAccessedFiles(int userId)
        {
            var model = new AccessedNotesViewModel();

            var accessedFileIds =
                _userSharedFileRepository.GetMany(usf => usf.UserId == userId).Select(f => f.FileId).ToList();
            var accessedFiles = _fileRepository.GetMany(f => accessedFileIds.Contains(f.FileId)).ToList();
            var sharingUserIds = accessedFiles.Select(u => u.UserId).Distinct().ToList();
            var users = _userRepository.GetMany(u => sharingUserIds.Contains(u.UserId));
            foreach (var file in accessedFiles)
            {
                if (users == null)
                {
                    return model;
                }

                var owner = users.FirstOrDefault(u => u.UserId == file.UserId);

                model.Notes.Add(new Note(file));
                model.Owners.Add(new SimpleUserModel(owner));
            }

            return model;
        }

        public bool IsPrivateFile(int fileId)
        {
            var file = _semesterSubjectFileRepository.GetMany(f => f.FileId == fileId);
            return !file.Any();
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
                _userSharedFileRepository.Delete(usf => usf.FileId == fileId && usf.UserId == userId);
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

        public void RemoveTagFromFile(int fileId, string tag)
        {
            var file = _fileRepository.GetById(fileId);
            var tags = file.FileTags.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList();

            tags.Remove(tag);
            var newTags = new StringBuilder();
            foreach (var tagPart in tags)
            {
                newTags.Append(tagPart);
                newTags.Append(",");
            }
            file.FileTags = newTags.ToString();
        }

        public void AddTagToFile(int fileId, string tag)
        {
            var file = _fileRepository.GetById(fileId);
            var newTags = new StringBuilder();
            newTags.Append(file.FileTags);
            newTags.Append(tag);
            newTags.Append(",");

            file.FileTags = newTags.ToString();
        }

        public bool FileHasTag(int fileId, string tag)
        {
            var file = _fileRepository.GetById(fileId);
            var tags = file.FileTags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < tags.Count; i++)
            {
                tags[i] = tags[i].ToLower();
            }
            return tags.Contains(tag.ToLower());
        }

        public bool TagExistsInDatabase(string tag)
        {
            var tagsInDatabase = _fileTagPatternRepository.GetMany(t => t.Name == tag);
            return tagsInDatabase.Any();
        }

        public void SaveFile()
        {
            _unitOfWork.Commit();
        }
    }
}
