using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class SubjectViewModel
    {
        private List<Subject> _subjectList;

        public SubjectViewModel()
        {
            _subjectList = new List<Subject>();
            Subjects = new Dictionary<string, bool>();
        }

        public SubjectViewModel(List<Subject> subjectList)
        {
            _subjectList = subjectList;
            Subjects = new Dictionary<string, bool>();

            foreach (var subject in _subjectList)
            {
                Subjects.Add(subject.Name, false);
            }
        }

        public Dictionary<string, bool> Subjects { get; set; } 
        //public Dictionary<Subject, bool> Subjects { get; set; }

        //public SubjectViewModel()
        //{
        //    Subjects = new Dictionary<Subject, bool>();
        //}
    }
}
