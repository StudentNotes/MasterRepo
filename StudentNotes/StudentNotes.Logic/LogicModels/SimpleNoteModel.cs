using StudentNotes.Logic.Consts;

namespace StudentNotes.Logic.LogicModels
{
    public class SimpleNoteModel
    {
        public int NoteId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public NoteType Type { get; set; }
    }
}
