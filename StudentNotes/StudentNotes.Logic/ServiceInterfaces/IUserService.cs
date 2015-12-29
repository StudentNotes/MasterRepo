using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ViewModels.Common;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface IUserService
    {
        bool UserExists(string email);
        bool UserExists(int userId);
        bool UserAuthorized(string email, string password);
        bool IsServiceAdmin(int userId);

        int GetServiceUserId(string email);
        int AddUser(string email, string password);
        int AddAdmin(string email, string password);
        string GetServiceUserName(int userid);
        string GetServiceUserLastName(int userId);

        UserInfo GetAllServiceUserInfo(int userId);
        UserPreferencesViewModel GetUserPreferences(int userId);

        void AddAvatar(int userId, string path);
        bool UpdateUserInfo(SecureUserModel model);
        bool UpdateUserPreferences(UserPreferencesViewModel model);
        void SaveUser();
    }
}
