using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using Xamarin.Forms;
using CloudClubv1._2_;

namespace FrontEnd
{
    public class FrontFriends
    {
        public string Emoji { get; set; }
        public string Username { get; set; }
        public Color SharedClubIndicator { get; set; }
        ColorHandler ch;
        public string Id { get; set; }
        public FrontFriends(Account account, bool inSameClub)
        {
            ch = new ColorHandler();
            Emoji = account.Emoji;
            Username = account.Username;
            Id = account.Id;
            if (inSameClub)
            {
                SharedClubIndicator = ch.fromStringToColor("green");
            }
            else
            {
                SharedClubIndicator = ch.fromStringToColor("green");
            }

        }
    }
}
