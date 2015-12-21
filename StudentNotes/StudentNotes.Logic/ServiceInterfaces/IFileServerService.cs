using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface IFileServerService
    {
        Task<int> UploadPrivateNote(Note note, int userId);
        Task<int> UploadUniversityNote(Note note, int userId, string filePath, int semesterSubjectId);
        Task<int> DeleteNote(int noteId);
        Task<int> DeletePrivateNoteAsync(int fileId);
        Task<int> DeleteSemesterSubjectNoteAsync(int fileId, int semesterSubjectId);
        Note DownloadNote(int noteId);
        void SaveUpload();
        void Commit();
    }
}
