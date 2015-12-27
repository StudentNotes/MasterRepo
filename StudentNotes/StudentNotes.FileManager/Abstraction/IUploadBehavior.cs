using System.Threading.Tasks;
using StudentNotes.FileManager.Base;

namespace StudentNotes.FileManager.Abstraction
{
    public interface IUploadBehavior
    {
        Task UploadFile(File file, FileServerClient client);
    }
}
