using StudentNotes.FileManager.Base;

namespace StudentNotes.FileManager.Abstraction
{
    public interface IDownloadBehavior<T>
    {
        T DownloadFile(string name, FileServerClient client);
    }
}
