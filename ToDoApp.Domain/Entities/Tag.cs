namespace ToDoApp.Domain.Entities
{
    public class Tag
    {
        private Tag() { } // Private constructor for EF Core 

        public Tag(string name, int ownerId)
        {
            this.Name = name;
            this.OwnerId = ownerId;
        }

        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public int OwnerId { get; private set; }
        public User Owner { get; private set; } = null!;
        public ICollection<NoteTag> NoteTags { get; private set; } = [];
        
        public void ChangeName( string newName )
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Tag name cannot be null, empty or whitespace.", nameof(newName));

            this.Name = newName;
        }
    }
}
