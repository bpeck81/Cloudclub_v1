using System;

namespace Backend
{
    public class Ban : DBItem
    {
        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string AccountId { get; set; }

        public string CommentId { get; set; }

        public Ban(string accountId, string commentId)
        {
            Time = DateTime.Now;
            AccountId = accountId;
            CommentId = commentId;
        }
    }
}