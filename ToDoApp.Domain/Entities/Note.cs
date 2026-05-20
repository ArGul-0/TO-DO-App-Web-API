namespace ToDoApp.Domain.Entities
{
    public class Note
    {
        private Note() { } // Private constructor for EF Core
        public Note(string title, string content, int userId)
        {
            Title = title;
            Content = content;
            UserId = userId;
        }

        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public string Content { get; private set; } = string.Empty;
        public bool IsDone { get; private set; } = false;
        public User User { get; private set; } = null!;
        public int UserId { get; private set; }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));

            Title = title;
        }

        public void UpdateContent(string content)
        {
            Content = content ?? string.Empty; // Allow content to be empty but not null
        }

        public void UpdateIsDone(bool isDone)
        {
            this.IsDone = isDone;
        }
    }
}
