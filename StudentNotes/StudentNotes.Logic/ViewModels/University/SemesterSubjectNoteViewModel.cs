using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentNotes.Logic.LogicModels;

namespace StudentNotes.Logic.ViewModels.University
{
    public class SemesterSubjectNoteViewModel
    {
        public int SemesterSubjectId { get; set; }
        public List<Note> NoteList { get; set; }

        public SemesterSubjectNoteViewModel()
        {
            NoteList = new List<Note>();
        }
    }
}
