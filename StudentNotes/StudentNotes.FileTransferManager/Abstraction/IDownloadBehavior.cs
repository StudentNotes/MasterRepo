using StudentNotes.FileTransferManager.Base;

namespace StudentNotes.FileTransferManager.Abstraction
{
    public interface IDownloadBehavior<T>
    {
        T DownloadFile(FileServerFile file, FileServer server, FileServerUser user);
    }
}
