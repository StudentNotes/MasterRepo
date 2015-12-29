using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class UserPreferencesViewModel
    {
        public int UserId { get; set; }
        public int NewestTime { get; set; }
        public string FileSize { get; set; }
        public string SearchMethod { get; set; }

        public UserPreferencesViewModel()
        {
        }

        public UserPreferencesViewModel(User user)
        {
            UserId = user.UserId;
            NewestTime = (user.UserPreferences.LastUploadDays ?? 0);
            FileSize = user.UserPreferences.MaxFileSize;
            SearchMethod = user.UserPreferences.SearchMethod;
        }
    }
}
