namespace ToDoApp.Infrastructure.Authentication
{
    public class JwtOptions
    {
        public string SecretKey { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public int ExpirationHours { get; init; }
        public string NameInCookies { get; init; } = null!;
    }
}
