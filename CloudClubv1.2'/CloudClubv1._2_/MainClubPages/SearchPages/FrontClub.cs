using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using Xamarin.Forms;

namespace FrontEnd
{
    public class FrontClub : ParentFrontClub
    {
        public string star1 { get; set; }
        public string star2 { get; set; }
        public string star3 { get; set; }
        public string star4 { get; set; }
        public string star5 { get; set; }
        public string mostRecentLine { get; set; }
        public string timeSinceActivity { get; set; }
        public bool isMember { get; set; }
        public bool isNotMemberNoPending { get; set; }
        private ColorHandler ch;
        public bool pendingInvite { get; set; }
        public FrontClub(Club club, bool member, bool pendingInvite, string mostRecentComment = "")
        {
            isMember = member;
            mostRecentLine = mostRecentComment;
            this.cloudId = club.CloudId;
            this.pendingInvite = pendingInvite;
            if (member == false && pendingInvite == false)
            {
                isNotMemberNoPending = true;
            }
            else
            {
                isNotMemberNoPending = false;
            }
            founderId = club.FounderId;
            Id = club.Id;
            ch = new ColorHandler();
            clubColor = club.Color;
            Title = club.Title;
            generateStarImages(club.GetRating());
            timeSinceActivity = getTimeSpan(club);
            starNumber = club.GetRating();



        }
        private void generateStarImages(int starNumber)
        {
            star1 = "Star_Empty.png";
            star2 = "Star_Empty.png";
            star3 = "Star_Empty.png";
            star4 = "Star_Empty.png";
            star5 = "Star_Empty.png";

            string starPath = ch.getStarColorString(clubColor);

            switch (starNumber)
            {
                case 0:
                    break;
                case 1:
                    star1 = starPath;
                    break;
                case 2:
                    star1 = starPath;
                    star2 = starPath;
                    break;
                case 3:
                    star1 = starPath;
                    star2 = starPath;
                    star3 = starPath;
                    break;
                case 4:
                    star1 = starPath;
                    star2 = starPath;
                    star3 = starPath;
                    star4 = starPath;
                    break;
                case 5:
                    star1 = starPath;
                    star2 = starPath;
                    star3 = starPath;
                    star4 = starPath;
                    star5 = starPath;
                    break;
            }


        }

        public string getTimeSpan(Club club)
        {
            string timeString = "";
            int timeInt = 0;
            var timeElapsed = club.GetTimeSinceActivity();

            if (-timeElapsed.Days >= 1)
            {
                var time = timeElapsed.Days.ToString() + " D";
                return time.Substring(1,time.Length-1);

            }
            else if (-timeElapsed.Hours >= 1)
            {
                var time = timeElapsed.Hours.ToString() + " Hr";
                return time.Substring(1,time.Length-1);
            }
            else if (-timeElapsed.Minutes >= 1)
            {
                var time = timeElapsed.Minutes.ToString() + " Min";
                return time.Substring(1,time.Length-1);
            }
            else return "<1 Min";

        }
    }
}
