using System;

namespace Backend
{
	/// A junction like class for friends (relationship between two accounts)
	/// NOTE: look through BOTH rows of the table to see if two people are friends, not just the author or recipient
    public class Friends : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string AuthorId{ get; set;}

		public string RecipientId{ get; set;}

		public Friends (string authorId, string recipientId){
			AuthorId = authorId;
			RecipientId = recipientId;
		}
	}
}

