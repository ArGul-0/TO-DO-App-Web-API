using ToDoApp.Domain.Enums;

namespace ToDoApp.Domain.Entities
{
    public class Friendship
    {
        private Friendship() { } // Private constructor for EF Core
        public Friendship(int requesterId, int addresseeId)
        {
            if (requesterId == addresseeId)
                throw new InvalidOperationException("Cannot friend yourself");

            RequesterId = requesterId;
            AddresseeId = addresseeId;
            Status = FriendshipStatus.Pending;
        }

        public int Id { get; private set; }

        public int RequesterId { get; private set; }
        public User Requester { get; private set; } = null!;

        public int AddresseeId { get; private set; }
        public User Addressee { get; private set; } = null!;

        public FriendshipStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public void Accept()
        {
            if(Status != FriendshipStatus.Pending)
            {
                throw new InvalidOperationException("Friendship status can only be changed from Pending.");
            }

            Status = FriendshipStatus.Accepted;
        }

        public void Reject()
        {
            if (Status != FriendshipStatus.Pending)
            {
                throw new InvalidOperationException("Friendship status can only be changed from Pending.");
            }

            Status = FriendshipStatus.Rejected;
        }
    }
}
