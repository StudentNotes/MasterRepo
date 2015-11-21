using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Logic.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IFileSharedGroupRepository _fileSharedGroupRepository;
        private readonly IGroupSemesterRepository _groupSemesterRepository;
        private readonly IFileRepository _fileRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISemesterSubjectRepository _semesterSubjectRepository;
        private readonly IStudySubjectRepository _studySubjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupService(IGroupRepository groupRepository, IGroupUserRepository groupUserRepository, IFileSharedGroupRepository fileSharedGroupRepository,
            IGroupSemesterRepository groupSemesterRepository, IFileRepository fileRepository, ISemesterRepository semesterRepository, ISemesterSubjectRepository semesterSubjectRepository,
            IUserRepository userRepository, IStudySubjectRepository studySubjectRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _groupUserRepository = groupUserRepository;
            _fileSharedGroupRepository = fileSharedGroupRepository;
            _groupSemesterRepository = groupSemesterRepository;
            _fileRepository = fileRepository;
            _semesterRepository = semesterRepository;
            _semesterSubjectRepository = semesterSubjectRepository;
            _userRepository = userRepository;
            _studySubjectRepository = studySubjectRepository;
            _unitOfWork = unitOfWork;
        }

        public Group GetGroupById(int groupId)
        {
            return _groupRepository.GetById(groupId);
        }

        public StudySubject GetStudySubjectByGroupId(int groupId)
        {
            var semester = GetGroupSemesters(groupId).FirstOrDefault();

            return _studySubjectRepository.GetById(semester.StudySubjectId);
        }

        public SecureUserModel GetGroupAdminDetails(int groupId)
        {
            var adminId = _groupRepository.GetById(groupId).AdminId;
            var user = _userRepository.GetById(adminId);
            var secureUser = new SecureUserModel()
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.UserInfo.Name,
                LastName = user.UserInfo.LastName,
                CreatedOn = user.UserInfo.CreatedOn,
                City = user.UserInfo.City,
                Country = user.UserInfo.Country,
                PhoneNumber = user.UserInfo.PhoneNumber,
                PostalCode = user.UserInfo.PostalCode,
                Profession = user.UserInfo.Profession,
                Street = user.UserInfo.Street
            };

            return secureUser;
        }

        public int AddGroup(string groupName, string description, int adminId, int semesterId)
        {
            _groupRepository.Add(new Group()
            {
                Name = groupName,
                Description = description,
                CreatedOn = DateTime.Now,
                AdminId = adminId,
                GroupSemester = new List<GroupSemester>()
                {
                    new GroupSemester()
                    {
                        SemesterId = semesterId
                    }
                }
            });

            return 0;
        }

        public int UpdateGroup(string name, string description, int groupId)
        {
            var group = _groupRepository.GetById(groupId);
            group.Description = description;
            group.Name = name;

            _groupRepository.Update(group);

            return 0;
        }

        public int AddUserToGroup(int userId, int groupId)
        {
            _groupUserRepository.Add(new GroupUser()
            {
                UserId = userId,
                GroupId = groupId
            });

            return 0;
        }

        public int AddFileToGroup(int fileId, int groupId, int semesterSubjectId)
        {
            _fileSharedGroupRepository.Add(new FileSharedGroup()
            {
                FileId = fileId,
                GroupId = groupId,
                SemesterSubjectId = semesterSubjectId
            });

            return 0;
        }

        public int AddSemesterToGroup(int semesterId, int groupId)
        {
            _groupSemesterRepository.Add(new GroupSemester()
            {
                GroupId = groupId,
                SemesterId = semesterId
            });

            return 0;
        }

        public int RemoveFileFromGroup(int fileId, int groupId)
        {
            _fileSharedGroupRepository.Delete(f => f.FileId == fileId && f.GroupId == groupId);

            return 0;
        }

        public int RemoveUserFromGroup(int userId, int groupId)
        {
            _groupUserRepository.Delete(u => u.UserId == userId && u.GroupId == groupId);

            return 0;
        }

        public int RemoveSemesterFromGroup(int semesterId, int groupId)
        {
            _groupSemesterRepository.Delete(s => s.SemesterId == semesterId && s.GroupId == groupId);

            return 0;
        }

        public int DeleteGroup(int groupId)
        {
            _groupUserRepository.Delete(gu => gu.GroupId == groupId);
            _groupSemesterRepository.Delete(gs => gs.GroupId == groupId);
            _fileSharedGroupRepository.Delete(fsg => fsg.GroupId == groupId);
            _groupRepository.Delete(g => g.GroupId == groupId);

            return 0;
        }

        public IEnumerable<Group> GetUserGroups(int userId)
        {
            var userGroupIds = _groupUserRepository.GetMany(gu => gu.UserId == userId).Select(g => g.GroupId).ToList();
            var groups = _groupRepository.GetMany(g => g.AdminId == userId || userGroupIds.Contains(g.GroupId));
            return groups;
        }

        public IEnumerable<Group> GetAdminGroups(int adminId)
        {
            var groups = _groupRepository.GetMany(g => g.AdminId == adminId);
            return groups;
        }

        public IEnumerable<File> GetGroupFiles(int groupId)
        {
            var groupFileIds =
                _fileSharedGroupRepository.GetMany(g => g.GroupId == groupId).Select(f => f.FileId).ToList();

            var file = _fileRepository.GetMany(f => groupFileIds.Contains(f.FileId));

            return file;
        }

        public IEnumerable<File> GetGroupSemesterFiles(int groupId, int semesterId)
        {
            var semester = _semesterRepository.GetById(semesterId);
            var semesterSubjectIds = semester.SemesterSubject.Select(ss => ss.SemesterSubjectId).ToList();
            var groupSemesterFileIds =
                _fileSharedGroupRepository.GetMany(
                    g => g.GroupId == groupId && semesterSubjectIds.Contains(g.SemesterSubjectId)).Select(f => f.FileId);

            var files = _fileRepository.GetMany(f => groupSemesterFileIds.Contains(f.FileId));
            return files;
        }

        public IEnumerable<File> GetGroupSemesterSubjectFiles(int groupId, int semesterSubjectId)
        {
            var semesterSubjectFileIds =
                _fileSharedGroupRepository.GetMany(
                    fg => fg.GroupId == groupId && fg.SemesterSubjectId == semesterSubjectId)
                    .Select(f => f.FileId)
                    .ToList();
            var files = _fileRepository.GetMany(f => semesterSubjectFileIds.Contains(f.FileId));
            return files;
        }

        public List<SecureUserModel> GetGroupUsers(int groupId)
        {
            var groupUserIds = _groupUserRepository.GetMany(gu => gu.GroupId == groupId).Select(u => u.UserId).ToList();
            var users = _userRepository.GetMany(u => groupUserIds.Contains(u.UserId));
            List<SecureUserModel> secureUserList = users.Select(user => new SecureUserModel()
            {
                UserId = user.UserId,
                Email = user.Email, 
                Name = user.UserInfo.Name, 
                LastName = user.UserInfo.LastName, 
                CreatedOn = user.UserInfo.CreatedOn, 
                City = user.UserInfo.City, 
                Country = user.UserInfo.Country, 
                PhoneNumber = user.UserInfo.PhoneNumber, 
                PostalCode = user.UserInfo.PostalCode, 
                Profession = user.UserInfo.Profession, 
                Street = user.UserInfo.Street
            }).ToList();

            return secureUserList;
        }

        public List<SecureUserModel> GetAllGroupUsers(int groupId)
        {
            var groupUserIds = _groupUserRepository.GetMany(gu => gu.GroupId == groupId).Select(u => u.UserId).ToList();
            groupUserIds.Add(_groupRepository.GetById(groupId).AdminId);

            var users = _userRepository.GetMany(u => groupUserIds.Contains(u.UserId));
            List<SecureUserModel> secureUserList = users.Select(user => new SecureUserModel()
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.UserInfo.Name,
                LastName = user.UserInfo.LastName,
                CreatedOn = user.UserInfo.CreatedOn,
                City = user.UserInfo.City,
                Country = user.UserInfo.Country,
                PhoneNumber = user.UserInfo.PhoneNumber,
                PostalCode = user.UserInfo.PostalCode,
                Profession = user.UserInfo.Profession,
                Street = user.UserInfo.Street
            }).ToList();

            return secureUserList;
        }

        public IEnumerable<Semester> GetGroupSemesters(int groupId)
        {
            var semesterIds =
                _groupSemesterRepository.GetMany(gs => gs.GroupId == groupId).Select(s => s.SemesterId).ToList();
            var semesters = _semesterRepository.GetMany(s => semesterIds.Contains(s.SemesterId));

            return semesters;
        }

        public Task<int> DownloadFileAsync(int fileId, int groupId)
        {
            throw new NotImplementedException();
        }

        public bool SemesterExists(int semesterId)
        {
            var semester = _semesterRepository.GetById(semesterId);
            return semester != null;
        }

        public bool GroupExists(int groupId)
        {
            var group = _groupRepository.GetById(groupId);
            return group != null;
        }

        public bool GroupInSemesterExists(string groupName, int semesterId)
        {
            var groupSemesterIds=
                _groupSemesterRepository.GetMany(gs => gs.SemesterId == semesterId).Select(gs => gs.GroupId).ToList();
            var group = _groupRepository.GetMany(g => g.Name == groupName && groupSemesterIds.Contains(g.GroupId));

            return group.Any();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
