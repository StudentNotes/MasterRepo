using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ServiceInterfaces
{
    public interface ISchoolService
    {
        School GetSchoolByName(string schoolName);
        IEnumerable<School> GetByUser(int userId);
        IEnumerable<School> GetAllSchhools();
        int AddSchool(string name, string description);

    }
}
