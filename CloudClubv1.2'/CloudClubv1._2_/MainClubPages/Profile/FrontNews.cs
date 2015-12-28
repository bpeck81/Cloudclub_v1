using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using CloudClubv1._2_;
using Xamarin.Forms;

namespace FrontEnd
{
    public class FrontNews
    {
        public string NotificationImage { get; set; }
        public string Text { get; set; }
        public string Time { get; set; }
        public string NotificationType { get; set; }
        public DBItem dbItem { get; set; }
        public FrontNews(string type, string text, DateTime? time, DBItem dbItem) 
        {
            this.dbItem = dbItem;
            NotificationType = type;
            var now = DateTime.Now;
            var timeSpan = (TimeSpan)(now - time);
            Time = getTimeSpan(timeSpan);
            Text = text;
            switch (type)
            {

                case "medal":
                    NotificationImage = "Medal_WhiteB";
                    break;
                case "droplet":
                    NotificationImage = "DropletFull_WhiteB.png";
                    break;
                case "rank":
                    NotificationImage = "Trophy_WhiteB.png";
                    break;
                case "warning":
                    NotificationImage = "Club_News.png";
                    break;
                case "join":
                    NotificationImage = "Club_News.png";
                    break;
                case "friend":
                    NotificationImage = "Medal_WhiteB.png";

                    break;
                case "ban":
                    NotificationImage = "AF36.png";
                    break;
                case "clubRequest":
                    NotificationImage = "Club_News.png";
                    break;
                case "invite":
                    NotificationImage = "Club_News.png";
                    break;
                case "friendRequest":
                    NotificationImage = "AF1.png";
                    Text = text + " has sent you a friend request";
                    break;
                default:
                    NotificationImage = "Club_News.png";
                    break;




            }


        }
        private string getTimeSpan(TimeSpan span)
        {
            string timeString = "";
            int timeInt = 0;
            var timeElapsed = span;
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
