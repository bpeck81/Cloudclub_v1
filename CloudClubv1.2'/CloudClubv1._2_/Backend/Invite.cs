﻿using System;

namespace Backend
{
    public class Invite : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string AuthorId{ get; set; }

		public string RecipientId{ get; set; }

		public string ClubId{ get; set; }

		public Invite (string authorId, string recipientId, string clubId){
			AuthorId = authorId;
			RecipientId = recipientId;
			ClubId = clubId;
		}
	}
}

