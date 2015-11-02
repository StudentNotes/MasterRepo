using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;

namespace StudentNotes.Repositories.RepositoryInterfaces
{
    public interface IFileTagPatternRepository : IRepository<FileTagPattern>
    {
    }
}
