using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; set; } = null!;
        public Email Email { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public List<Note> Notes { get; private set; } = new List<Note>();

        public Note AddNote(string title, string content)
        {
            var note = new Note(title, content, this);

            Notes.Add(note);

            return note;
        }

        public void UpdateUsername(string newUsername)
        {
            if(string.IsNullOrEmpty(newUsername)) 
                throw new ArgumentException("Username cannot be null or empty.", nameof(newUsername));

            Username = newUsername;
        }

        public void UpdateEmail(Email newEmail)
        {
            if(newEmail == null) 
                throw new ArgumentNullException(nameof(newEmail), "Email cannot be null.");

            Email = newEmail;
        }
    }
}
