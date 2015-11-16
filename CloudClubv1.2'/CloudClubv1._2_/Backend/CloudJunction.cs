using System;

namespace Backend
{
    public class CloudJunction : DBItem
    {
        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string AccountId { get; set; }

        public string CloudId { get; set; }

        public CloudJunction(string cloudId, string userId)
        {
            Time = DateTime.Now;
            AccountId = userId;
            CloudId = cloudId;
        }
    }
}

