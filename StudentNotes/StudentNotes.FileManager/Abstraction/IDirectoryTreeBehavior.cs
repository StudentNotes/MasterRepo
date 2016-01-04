using StudentNotes.FileManager.Base;

namespace StudentNotes.FileManager.Abstraction
{
    public interface IDirectoryTreeBehavior
    {
        void GoToOrCreatePath(string path, FileServerClient client);
        void GoToPath(string path, FileServerClient client);
        bool FileOrDirectoryAlreadyExists(string fileName, FileServerClient client);
        string GetNewNameForFile(string oldName, FileServerClient client);
    }
}
