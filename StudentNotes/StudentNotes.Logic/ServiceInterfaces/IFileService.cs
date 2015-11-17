using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface IFileService
    {
        IEnumerable<File> GetPrivateFiles(int userId);
        IEnumerable<File> GetFilesBySemesterSubjectId(int semesterSubjectId);
        IEnumerable<File> GetSemesterSubjectFilesByUserId(int semesterSubjectId, int userId);
        IEnumerable<File> GetSharedUserFiles(int userId);
        File GetFileById(int fileId);
        void SaveFile();
    }
}
