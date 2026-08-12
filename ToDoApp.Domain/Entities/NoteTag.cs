namespace ToDoApp.Domain.Entities
{
    public class NoteTag
    {
        private NoteTag() { } // Private constructor for EF Core
        public NoteTag(int noteId, int tagId)
        {
            this.NoteId = noteId;
            this.TagId = tagId;
        }

        public int NoteId { get; private set; }
        public Note Note { get; private set; } = null!;
        public int TagId { get; private set; }
        public Tag Tag { get; private set; } = null!;
    }
}
