using System.Collections.Generic;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Common
{
    public class SubjectViewModel
    {
        public Dictionary<string, bool> Subjects { get; set; }

        public SubjectViewModel()
        {
            Subjects = new Dictionary<string, bool>();
        }

        public SubjectViewModel(List<Subject> subjectList)
        {
            Subjects = new Dictionary<string, bool>();

            foreach (var subject in subjectList)
            {
                Subjects.Add(subject.Name, false);
            }
        }
    }
}
