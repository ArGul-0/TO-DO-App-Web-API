namespace ToDoApp.Domain.Entities
{
    public class Note
    {
        private Note() { } // Private constructor for EF Core
        public Note(string title, string content, bool isDone, int userId)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content), "Content cannot be null or empty.");
            if (isDone != true && isDone != false)
                throw new ArgumentException("IsDone must be a boolean value.", nameof(isDone));
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive integer.", nameof(userId));

            Title = title;
            Content = content;
            IsDone = isDone;
            UserId = userId;
        }

        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string Content { get; private set; } = string.Empty;
        public bool IsDone { get; private set; } = false;
        public ICollection<NoteTag> NoteTags { get; private set; } = [];
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public User User { get; private set; } = null!;
        public int UserId { get; private set; }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));

            Title = title;

            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateContent(string content)
        {
            Content = content ?? string.Empty; // Allow content to be empty but not null

            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateIsDone(bool isDone)
        {
            this.IsDone = isDone;

            UpdatedAt = DateTime.UtcNow;
        }

        public void AddTag(NoteTag tag)
        {
            if (tag is null)
                throw new ArgumentNullException(nameof(tag), "Tag cannot be null.");

            NoteTags.Add(tag);
        }

        public void RemoveTag(NoteTag tag)
        {
            if (tag is null)
                throw new ArgumentNullException(nameof(tag), "Tag cannot be null.");

            NoteTags.Remove(tag);
        }
    }
}
