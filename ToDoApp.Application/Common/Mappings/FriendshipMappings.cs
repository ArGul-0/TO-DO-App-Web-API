using ToDoApp.Application.DTOs;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Mappings
{
    public static class FriendshipMappings
    {
        extension(Friendship friendship)
        {
            public FriendshipDto ToDto()
            {
                return new FriendshipDto(
                    Id: friendship.Id,
                    RequesterId: friendship.RequesterId,
                    RequesterUsername: friendship.Requester.Username,
                    AddresseeId: friendship.AddresseeId,
                    AddresseeUsername: friendship.Addressee.Username,
                    Status: friendship.Status,
                    CreatedAt: friendship.CreatedAt
                );
            }
        }
    }
}