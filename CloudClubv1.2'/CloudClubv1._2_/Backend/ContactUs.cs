using System;

namespace Backend
{
    public class ContactUs : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string AuthorId{ get; set;}

		public string Text{ get; set;}

		public ContactUs (string userId, string text){
			AuthorId = userId;
			Text = text;
		}
	}
}

