using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;
using StudentNotes.Repositories.Infrastructure;

namespace StudentNotes.Repositories.Base
{
    public class DbFactory : Disposable, IDbFactory
    {
        private StudentNotesContext _dbContext;

        public StudentNotesContext Init()
        {
            return _dbContext ?? (_dbContext = new StudentNotesContext());
        }

        protected override void DisposeCore()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }
    }
}
