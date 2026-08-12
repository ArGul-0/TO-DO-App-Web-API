using ToDoApp.Domain.Enums;
using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Domain.Entities
{
    public class User
    {
        private User() { } // Private constructor for EF Core
        public User(string username, Email email, string hashedPassword)
        {
            if(string.IsNullOrEmpty(username)) 
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            if(email is null) 
                throw new ArgumentNullException(nameof(email), "Email cannot be null.");
            if(string.IsNullOrEmpty(hashedPassword)) 
                throw new ArgumentException("Password cannot be null or empty.", nameof(hashedPassword));

            Username = username;
            Email = email;
            HashedPassword = hashedPassword;
        }
        public int Id { get; private set; }
        public string Username { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public string HashedPassword { get; private set; } = null!;
        public ICollection<Note> Notes { get; private set; } = new List<Note>();
        public ICollection<Tag> Tags { get; private set; } = new List<Tag>();
        public AccountVisibility Visibility { get; private set; } = AccountVisibility.Public;

        public Note AddNote(string title, string content, bool isDone = false)
        {
            var note = new Note(title, content, isDone, Id);

            Notes.Add(note);

            return note;
        }

        public Tag AddTag(string name)
        {
            var tag = new Tag(name, Id);

            Tags.Add(tag);

            return tag;
        }

        public void UpdateUsername(string newUsername)
        {
            if(string.IsNullOrEmpty(newUsername)) 
                throw new ArgumentException("Username cannot be null or empty.", nameof(newUsername));

            Username = newUsername;
        }

        public void UpdateEmail(Email newEmail)
        {
            if(newEmail is null) 
                throw new ArgumentNullException(nameof(newEmail), "Email cannot be null.");

            Email = newEmail;
        }

        public void ChangeAccountVisibility(AccountVisibility visibility) => Visibility = visibility;
    }
}
