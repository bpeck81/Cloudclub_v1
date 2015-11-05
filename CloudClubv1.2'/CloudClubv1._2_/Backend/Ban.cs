using System;

namespace Backend
{
    public class Ban : DBItem
    {
        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string AccountId { get; set; }

        public string CommentId { get; set; }

        public string ReporterId { get; set; }

        public Ban(string accountId, string commentId, string reporterId)
        {
            Time = DateTime.Now;
            AccountId = accountId;
            CommentId = commentId;
            ReporterId = reporterId;
        }
    }
}