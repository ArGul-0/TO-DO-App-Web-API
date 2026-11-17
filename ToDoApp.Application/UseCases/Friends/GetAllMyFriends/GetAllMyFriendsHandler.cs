using ToDoApp.Application.Common;
using ToDoApp.Application.Common.Mappings;
using ToDoApp.Application.DTOs;
using ToDoApp.Application.Interfaces.Repositories;

namespace ToDoApp.Application.UseCases.Friends.GetAllMyFriendships
{
    public class GetAllMyFriendsHandler
    {
        private readonly IFriendshipRepository friendshipRepository;
        public GetAllMyFriendsHandler(IFriendshipRepository friendshipRepository)
        {
            this.friendshipRepository = friendshipRepository;
        }

        public async Task<ResultT<List<FriendDto>>> Handle(int userId)
        {
            var friendships = await friendshipRepository.GetAllFriendshipsByUserIdAsync(userId);

            var friendshipDtos = friendships.Select(f => f.ToDto()).ToList();

            return ResultT<List<FriendDto>>.Success(friendshipDtos);
        }
    }
}
