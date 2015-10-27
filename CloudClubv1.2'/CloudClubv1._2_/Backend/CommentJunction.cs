using System;

namespace Backend
{


	/// Records what users have rated what comments
    public class CommentJunction : DBItem
	{

		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string RaterId{ get; set;}

		public string CommentId{ get; set;}

		public CommentJunction (string raterId, string commentId){
			RaterId = raterId;
			CommentId = commentId;
		}
	}
}

