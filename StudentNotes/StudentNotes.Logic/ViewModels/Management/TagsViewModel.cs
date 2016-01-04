using System.Collections.Generic;
using StudentNotes.Repositories.DbModels;

namespace StudentNotes.Logic.ViewModels.Management
{
    public class TagsViewModel
    {
        public Dictionary<string, bool> Tags { get; set; }

        public TagsViewModel()
        {
            Tags = new Dictionary<string, bool>();
        }

        public TagsViewModel(List<FileTagPattern> subjectList)
        {
            Tags = new Dictionary<string, bool>();

            foreach (var subject in subjectList)
            {
                Tags.Add(subject.Name, false);
            }
        }
    }
}
