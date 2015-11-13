using System;

namespace Backend
{
    public class ClubReqRatingJunc : DBItem
    {
        //NOTE: since club requests are accepted on the first accept, clubreqratingjunc items are inherently negative - 
        //they are only created on dedclines 

        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string ClubRequestId { get; set; }

        public string RaterId { get; set; }

        public ClubReqRatingJunc(string clubRequestId, string raterId)
        {
            Time = DateTime.Now;

            ClubRequestId = clubRequestId;
            RaterId = raterId;
        }
    }
}

