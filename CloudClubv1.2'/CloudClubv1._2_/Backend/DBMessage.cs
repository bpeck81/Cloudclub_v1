using System;

namespace Backend
{
    public class DBMessage : DBItem
    {

        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string Text { get; set; }

        public string AuthorId { get; set; }

        public string RecipientId { get; set; }

        public DBMessage(string text, string authorId, string recipientId)
        {
            Text = text;
            AuthorId = authorId;
            RecipientId = recipientId;
        }
    }
}

