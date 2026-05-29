using System.ComponentModel.DataAnnotations;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.DTOs
{
    public record FriendshipDto(
        int Id,
        int RequesterId,
        string RequesterUsername,
        int AddresseeId,
        string AddresseeUsername,
        FriendshipStatus Status,
        DateTime CreatedAt
        );
}
