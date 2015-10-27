using System;

namespace Backend
{
    public class DBNotification : DBItem
    {
        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string AccountId { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }

        public DBNotification(string accountId, string type, string text)
        {
            Time = DateTime.Now;
            AccountId = accountId;
            Type = type;
            Text = text;
        }
    }
}