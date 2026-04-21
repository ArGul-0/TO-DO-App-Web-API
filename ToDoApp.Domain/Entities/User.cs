using ToDoApp.Domain.ValueObjects;

namespace ToDoApp.Domain.Entities
{
    public class User
    {
        public int Id { get; }
        public required string Username { get; set; }
        public required Email Email { get; set; }
        public required string HashedPassword { get; set; }

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
