using System.Collections.Generic;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface IFileService
    {
        List<SecureUserModel> GetUsersWithFileAccess(int fileId);
        IEnumerable<File> GetPrivateFiles(int userId);
        IEnumerable<File> GetFilesBySemesterSubjectId(int semesterSubjectId);
        IEnumerable<File> GetSemesterSubjectFilesByUserId(int semesterSubjectId, int userId);
        IEnumerable<File> GetSharedUserFiles(int userId);
        List<File> GetSharedGroupFiles(int userId);
        File GetFileById(int fileId);
        SecureUserModel GetSecureUser(int userId);

        bool IsPrivateFile(int fileId);
        bool UserHasAccess(int fileId, int userId);
        int AddFileToUser(int fileId, int userId);
        int AddFileToUser(int fileId, string email);
        int RemoveFileFromUser(int fileId, int userId);
        int RemoveFileFromUser(int fileId, string email);

        void SaveFile();
    }
}
