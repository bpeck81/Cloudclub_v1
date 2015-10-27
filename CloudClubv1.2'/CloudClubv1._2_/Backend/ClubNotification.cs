using System;

namespace Backend
{
    public class ClubNotification : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string CommentId{ get; set;}

		public string ClubId{ get; set;}

		public ClubNotification (){}
	}
}

