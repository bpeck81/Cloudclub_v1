using System;

namespace Backend
{
	/// records what users have rated what clubs
    public class RatingJunction : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string AccountId{ get; set;}

		public string ClubId{ get; set;}

		public int Rating{ get; set;}

		public RatingJunction (int rating, string accountId, string clubId){
			Rating = rating;
			AccountId = accountId;
			ClubId = clubId;
		}
	}
}

