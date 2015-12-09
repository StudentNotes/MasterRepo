using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface IGroupService
    {
        Group GetGroupById(int groupId);
        StudySubject GetStudySubjectByGroupId(int groupId);
        SecureUserModel GetGroupAdminDetails(int groupId);
        Semester GetSemesterBySemesterSubject(int semesterSubjectId);
        GroupBasics GetGroupBasics(int groupId, int fileId);
        List<GroupBasics> GetGroupsWithFileAccess(int fileId); 

        int AddGroup(string groupName, string description, int adminId, int semesterId);
        int UpdateGroup(string name, string description, int groupId);
        int AddUserToGroup(int userId, int groupId);
        int AddFileToGroup(int fileId, int groupId, int semesterSubjectId);
        int AddSemesterToGroup(int semesterId, int groupId);
        int RemoveFileFromGroup(int fileId, int groupId);
        int RemoveUserFromGroup(int userId, int groupId);
        int RemoveSemesterFromGroup(int semesterId, int groupId);
        int DeleteGroup(int groupId);

        IEnumerable<Group> GetUserGroups(int userId);
        IEnumerable<Group> GetAdminGroups(int adminId);
        IEnumerable<File> GetGroupFiles(int groupId);
        IEnumerable<File> GetGroupSemesterFiles(int groupId, int semesterId);
        IEnumerable<File> GetGroupSemesterSubjectFiles(int groupId, int semesterSubjectId);
        List<SecureUserModel> GetGroupUsers(int groupId);
        List<SecureUserModel> GetAllGroupUsers(int groupId);
        IEnumerable<Semester> GetGroupSemesters(int groupId);

        Task<int> DownloadFileAsync(int fileId, int groupId);

        bool SemesterExists(int semesterId);
        bool GroupExists(int groupId);
        bool GroupInSemesterExists(string groupName, int semesterId);
        bool SemesterSubjectExists(int semesterSubjectId);
        bool FileSharedToGroup(int fileId, int groupId, int semesterSubjectId);

        void Commit();
    }
}
