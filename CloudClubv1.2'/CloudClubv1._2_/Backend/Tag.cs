using System;

namespace Backend
{
	public class Tag:DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string Key{get;set;}

		public string ClubId{ get; set; }

        public string CloudId { get; set; }

		public Tag (string key, string clubId, string cloudId){
			Key = key;
			ClubId = clubId;
            CloudId = cloudId;
		}

	}
}

