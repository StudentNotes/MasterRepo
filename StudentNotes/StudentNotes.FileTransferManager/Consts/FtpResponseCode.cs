
namespace StudentNotes.FileTransferManager.Consts
{
    public enum FtpResponseCode
    {
        FileUploaded = 226,
        UserLoggedIn = 230,
        UserLoggedOut = 231,
        FileDeleted = 250,
        FolderCreated = 257,
        FolderExists = 258,
        NotLoggedIn = 530,
        FileNotFoundOrNoAccess = 550,
        GlobalError = 666,
        CommandsExecutedSuccessfully = 777,

        FileDoesntExist = 667
    }
}