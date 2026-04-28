namespace ToDoApp.Domain.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string Content { get; set; } = string.Empty;

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
    }
}
