using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface IUploadService
    {
        Task<int> UploadPrivateNote(Note note, int userId);
        Task<int> UploadUniversityNote(Note note, int userId, string filePath, int semesterSubjectId);
        Task<int> DeletePrivateNoteAsync(int fileId);
        Task<int> DeleteSemesterSubjectNoteAsync(int fileId, int semesterSubjectId);
        void SaveUpload();
        void Commit();
    }
}
