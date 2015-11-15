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
        int UploadPrivateNote(Note note, int userId);
        int UploadUniversityNote(Note note, int userId, string filePath, int semesterSubjectId);
        void SaveUpload();
        void Commit();
    }
}
