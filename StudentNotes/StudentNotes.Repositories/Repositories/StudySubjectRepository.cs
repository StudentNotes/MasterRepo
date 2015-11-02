using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.Base;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.Repositories.Repositories
{
    public class StudySubjectRepository : RepositoryBase<StudySubject>, IStudySubjectRepository
    {
        public StudySubjectRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
