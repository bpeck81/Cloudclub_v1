using System;

namespace Backend
{
    public class Comment : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string AuthorId{ get; set;}

		public string Text{ get; set;}

		public int NumDroplets{ get; set;}

		public string ClubId{ get; set;}

        public bool Picture { get; set; }

		public Comment (string userId, string clubId, string text){
			NumDroplets = 0;
			AuthorId = userId;
			ClubId = clubId;
			Text = text;
            Picture = false;
		}
	}
}

