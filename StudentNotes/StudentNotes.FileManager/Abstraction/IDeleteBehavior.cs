using System.Threading.Tasks;
using StudentNotes.FileManager.Base;

namespace StudentNotes.FileManager.Abstraction
{
    public interface IDeleteBehavior
    {
        Task DeleteFile(string name, FileServerClient client);
        Task DeleteDirectory(string name, FileServerClient client);
    }
}
