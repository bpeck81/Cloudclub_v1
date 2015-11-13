using System;

namespace Backend
{
	public class Account:DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string Username{ get; set;}

		public string Password{get;set;}

		public string Emoji{ get; set;}

		public string Color{ get; set;}

		public string Description{ get; set;}

		public int NumDroplets{ get; set;}

		public int NumClubsCreated{ get; set;}

		public int TotalClubsRatings{ get; set;}

		public int Ranking{ get; set;}

		public int NumClubsIn{ get; set;}

		public int NumComments{ get; set;}

		public int NumFriends{ get; set;}

        public int Points { get; set; }

        //the user is banned until the banned time (less of a ban and more of a mute)
        //thus, if the banned time is in the past, it doesn't effect the user, so it is initialized to when
        //the user account is created
        public DateTime Banned { get; set; }

        public bool RatingNotificationToggle { get; set; }

        public int NumMedals { get; set; }

        public string Email { get; set; }

        public int NumInvites { get; set; }

		public Account (string username, string password, string email){
			//datetime is null, passed in by server
			Emoji = "none";
			Color = "none";
			Description = "Edit to add description.";
			NumDroplets = 0;
			NumClubsCreated = 0;
			TotalClubsRatings = 0;
			Ranking = 0;
			NumClubsIn = 0;
			NumComments = 0;
			NumFriends = 0;
            Points = 0;
            Banned = DateTime.Now;
            RatingNotificationToggle = false;
            NumMedals = 0;
            NumInvites = 0;

			Username = username;
			Password = password;
            Email = email;
		}


	}
}

