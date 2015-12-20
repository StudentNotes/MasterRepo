using StudentNotes.Repositories.Base;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Repositories.Repositories
{
    public class UserPreferencesRepository : RepositoryBase<UserPreferences>, IUserPreferencesRepository
    {
        public UserPreferencesRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
