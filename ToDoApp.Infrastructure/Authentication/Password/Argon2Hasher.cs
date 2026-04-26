using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using ToDoApp.Application.Interfaces;

namespace ToDoApp.Infrastructure.Authentication.Password
{
    public class Argon2Hasher : IPasswordHasher
    {
        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private static readonly int DegreeOfParallelism = Math.Min(Environment.ProcessorCount, 4); // Number of Threads To Use
        private const int Iterations = 4; // Number Of Iterations
        private const int MemorySize = 1024 * 64; // Memory Size In KB (64 MB)

        public async Task<string> HashPasswordAsync(string password)
        {
            if (password is null) throw new ArgumentNullException(nameof(password)); // Validate Input

            try
            {
                // Generate A Random Salt
                byte[] salt = new byte[SaltSize];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // Create An Argon2 Instance With The Provided Value And Configured Parameters
                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = DegreeOfParallelism,
                    Iterations = Iterations,
                    MemorySize = MemorySize
                };

                byte[] hash = await argon2.GetBytesAsync(HashSize);

                // Combine The Salt And Hash For Storage (Base64 Encoding)
                var combinedBytes = new byte[salt.Length + hash.Length];
                Buffer.BlockCopy(salt, 0, combinedBytes, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, combinedBytes, salt.Length, hash.Length);

                return Convert.ToBase64String(combinedBytes);
            }
            catch (Exception ex)
            {
                // Log The Exception (Logging Not Implemented Here)
                throw new InvalidOperationException("An error occurred while hashing the value.", ex);
            }
        }

        public async Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
        {
            if (password is null) throw new ArgumentNullException(nameof(password)); // Validate Input
            if (hashedPassword is null) throw new ArgumentNullException(nameof(hashedPassword)); // Validate Input

            try
            {
                // Decode The Stored Hash
                byte[] combinedBytes = Convert.FromBase64String(hashedPassword);

                // Extract The Salt And Hash
                byte[] salt = new byte[SaltSize];
                byte[] hash = new byte[HashSize];
                Buffer.BlockCopy(combinedBytes, 0, salt, 0, SaltSize);
                Buffer.BlockCopy(combinedBytes, SaltSize, hash, 0, HashSize);

                // Create An Argon2 Instance With The Provided Value And Extracted Salt
                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = DegreeOfParallelism,
                    Iterations = Iterations,
                    MemorySize = MemorySize
                };

                byte[] computedHash = await argon2.GetBytesAsync(HashSize);

                // Compare The Computed Hash With The Stored Hash
                return CryptographicOperations.FixedTimeEquals(computedHash, hash);
            }
            catch (FormatException)
            {
                // Log The Exception (Logging Not Implemented Here)
                throw new InvalidOperationException("The provided hashed value is not in a valid format.");
            }
            catch (Exception ex)
            {
                // Log The Exception (Logging Not Implemented Here)
                throw new InvalidOperationException("An error occurred while verifying the value.", ex);
            }
        }
    }
}
