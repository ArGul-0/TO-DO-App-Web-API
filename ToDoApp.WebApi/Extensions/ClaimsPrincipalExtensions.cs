using System.Security.Claims;

namespace ToDoApp.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        extension(ClaimsPrincipal user)
        {
            public int GetUserId()
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                    throw new InvalidOperationException("User ID claim not found.");

                if (!int.TryParse(userIdClaim.Value, out var userId))
                    throw new InvalidOperationException("User ID claim is not a valid integer.");

                return userId;
            }
        }
    }
}