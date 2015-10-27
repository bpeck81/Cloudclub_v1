using System;

namespace Backend
{
    public class ClubRequest : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string AccountId{ get; set;}

		public string ClubId{ get; set;}

		public string Text{get;set;}

		public ClubRequest (string accountId, string clubId, string text){
			AccountId = accountId;
			ClubId = clubId;
			Text = text;
		}
	}
}

