using System.Net.Mail;

namespace ToDoApp.Domain.ValueObjects
{
    internal class Email
    {
        public string Value { get; }
        public Email(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Email cannot be null or empty.", nameof(value));
            else if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email format.", nameof(value));

            Value = value;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
