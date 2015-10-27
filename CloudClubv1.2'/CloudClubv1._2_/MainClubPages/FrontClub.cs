using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using Xamarin.Forms;

namespace FrontEnd
{
    public class FrontClub
    {
        public string star1 { get; set; }
        public string star2 { get; set; }
        public string star3 { get; set; }
        public string star4 { get; set; }
        public string star5 { get; set; }
        public string clubColor { get; set; }
        public string mostRecentLine { get; set; }
        public string Title{get; set;}
        public string timeSinceActivity { get; set; }
        public bool isMember { get; set; }
        public bool isNotMember { get; set; }
        public string Id { get; set; }
        private ColorHandler ch;
        public FrontClub(Club club, bool member)
        {
            isMember = member;
            isNotMember = !isMember;
            Id = club.Id;
            ch = new ColorHandler();
            clubColor = club.Color;
            Title = club.Title;
            generateStarImages(club.GetRating());
            timeSinceActivity = getTimeSpan(club);
            mostRecentLine = "Most recent line" + "...";
            
            
            
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
            if (timeElapsed.TotalMinutes > 60)
            {
                if (timeElapsed.TotalHours > 24)
                {
                    timeInt = (int)timeElapsed.TotalDays;
                    timeString = timeInt.ToString() + "d";
                }
                else
                {
                    timeInt = (int)timeElapsed.TotalHours;
                    timeString = timeInt.ToString() + "hr";

                }
            }
            else
            {
                timeInt = (int)timeElapsed.TotalMinutes;
                timeString = timeInt.ToString() + "m";
            }

            return timeString;
        }
    }
}
