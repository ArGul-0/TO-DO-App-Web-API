namespace ToDoApp.Application.Interfaces
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes the provided password using a secure hashing algorithm. The resulting hash is typically stored in the database instead of the plain text password, enhancing security by making it difficult for attackers to retrieve the original password even if they gain access to the hashed values.
        /// </summary>
        /// <param name="password">The plain text password to be hashed.</param>
        /// <returns>The hashed representation of the provided password.</returns>
        public Task<string> HashPasswordAsync(string password);
        /// <summary>
        /// Verifies whether the provided plain text password matches the given hashed password.
        /// </summary>
        /// <param name="password">The plain text password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <returns>True if the password matches the hashed password; otherwise, false.</returns>
        public Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
    }
}
