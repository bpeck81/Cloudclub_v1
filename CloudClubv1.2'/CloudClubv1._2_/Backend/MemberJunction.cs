using System;

namespace Backend
{
    public class MemberJunction : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string AccountId{ get; set;}

		public string ClubId{ get; set;}

		public MemberJunction (string userId,string clubId){
			AccountId = userId;
			ClubId = clubId;
		}
	}
}

