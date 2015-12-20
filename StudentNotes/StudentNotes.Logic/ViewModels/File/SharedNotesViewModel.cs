using StudentNotes.Logic.Consts;

namespace StudentNotes.Logic.ViewModels.File
{
    public class SharedNotesViewModel
    {
        public int NoteId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public NoteType Type { get; set; }
    }
}
