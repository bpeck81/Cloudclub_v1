using System;

namespace Backend
{
    ///NOTE: this class is purely for push notification updates for non club members
    public class TemporaryMemberJunction : DBItem
    {
        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string AccountId { get; set; }

        public string ClubId { get; set; }

        public TemporaryMemberJunction(string userId, string clubId)
        {
            Time = DateTime.Now;
            AccountId = userId;
            ClubId = clubId;
        }
    }
}

