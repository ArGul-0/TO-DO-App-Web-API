using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a JWT access token for the given user. The token typically includes claims such as the user's ID, username, and email, and is signed using a secret key to ensure its integrity and authenticity.
        /// </summary>
        /// <param name="user" >The user for whom the token is being generated.</param>
        /// <returns>The generated JWT access token as a string.</returns>
        public string GenerateAccessToken(User user);

    }
}
