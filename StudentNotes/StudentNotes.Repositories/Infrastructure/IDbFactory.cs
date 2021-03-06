﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Repositories.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        StudentNotesContext Init();
    }
}
