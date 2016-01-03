using System.Collections.Generic;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;

namespace StudentNotes.Repositories.RepositoryInterfaces
{
    public interface IFileRepository : IRepository<File>
    {
        List<File> GetByTag(string tags);
    }
}
