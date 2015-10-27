using System;

namespace Backend
{
    public class Club : DBItem
	{
		public string Id{ get; set;}

		public DateTime? Time{ get; set;}

		public string Color{ get; set; }

		public string Title{ get; set; }

		public DateTime? LatestActivity{ get; set; }

		public int NumRatings{ get; set; }

		public int TotalRating{ get; set; }

		//TODO: should variables that track numbers even be stored? it's bad practice, they should be calculated
		public int NumMembers{ get; set; }

		public bool Exclusive{ get; set; }

		public string FounderId{ get; set; }

		public Club (string title, string color, bool exclusive, string founderId){
			//Note: tags have their own table
			NumRatings = 0;
			TotalRating = 0;
			NumMembers = 1;

			Title = title;
			Color = color;
			Exclusive = exclusive;
			FounderId = founderId;
		}


		//NOTE: it's bad practice to store calculated values into a database, so these functions return values that must be caluclated

		/// Returns the time since the clubs latest post as a TimeSpan instance
		public TimeSpan GetTimeSinceActivity(){
			return (TimeSpan)(LatestActivity-DateTime.Now);
		}

		/// Returns the an int of the club's rating
		public int GetRating(){
			//make sure to not divide by zero
			if(NumRatings==0){
				return 0;
			}else{
				return (int)(TotalRating/NumRatings);
			}
		}


	}
}

