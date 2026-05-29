using System.ComponentModel.DataAnnotations;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.DTOs
{
    public record FriendshipDto(
        [Required] int Id,
        [Required] int RequesterId,
        [Required] string RequesterUsername,
        [Required] int AddresseeId,
        [Required] string AddresseeUsername,
        [Required] FriendshipStatus Status
        );
}
