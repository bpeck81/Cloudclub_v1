using System;

namespace Backend
{
	public class Tag:DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string Key{get;set;}

		public string ClubId{ get; set; }

		public Tag (string key, string clubId){
			Key = key;
			ClubId = clubId;
		}

	}
}

