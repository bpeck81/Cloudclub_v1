using System;

namespace Backend
{
    public class ClubReport : DBItem
    {
        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string ClubId { get; set; }

        public string ReporterId { get; set; }

        //this is bad practice since I have a reference to the club, but im storing the title
        //sinc i really don't want to deal with node.js right now :p
        public string ClubTitle { get; set; }

        public ClubReport(string clubId, string reporterId, string clubTitle)
        {
            Time = DateTime.Now;
            ClubId = clubId;
            ReporterId = reporterId;
            ClubTitle = clubTitle;
        }
    }
}