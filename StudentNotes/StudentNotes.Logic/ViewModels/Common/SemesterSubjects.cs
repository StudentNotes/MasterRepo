﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class SemesterSubjects
    {
        public int SemesterId { get; set; }
        public List<SemesterSubject> SemesterSubjectList { get; set; }

        public SemesterSubjects()
        {
            SemesterSubjectList = new List<SemesterSubject>();
        }
    }
}