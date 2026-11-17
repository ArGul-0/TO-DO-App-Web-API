using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Infrastructure.Authentication.Jwt
{
    public class JwtOptions
    {
        [Required]
        [MinLength(32)]
        public string SecretKey { get; init; } = null!;

        [Required]
        public string Issuer { get; init; } = null!;

        [Required]
        public string Audience { get; init; } = null!;

        [Range(1, int.MaxValue)]
        public int ExpirationHours { get; init; }

        [Required]
        public string NameInCookies { get; init; } = null!;
    }
}
