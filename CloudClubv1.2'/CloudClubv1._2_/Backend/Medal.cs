using System;

namespace Backend
{
    public class Medal : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string AccountId{ get; set; }

		public string MedalName{get;set;}

		public int Points{ get; set; }

		public Medal (string medalName, string accountId, int points){
            MedalName = medalName;
            AccountId = accountId;
            Points = points;
        }
	}
}

