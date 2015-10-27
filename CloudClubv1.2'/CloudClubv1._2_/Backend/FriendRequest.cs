using System;

namespace Backend
{
    public class FriendRequest : DBItem
	{

		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string AuthorId{ get; set;}

		public string RecipientId{ get; set;}

		public FriendRequest (string authorId, string recipientId){
			AuthorId = authorId;
			RecipientId = recipientId;
		}
	}
}

