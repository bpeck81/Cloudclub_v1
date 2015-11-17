using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CloudClubv1._2_;
using Backend;
namespace FrontEnd
{
    public class FrontClubMember
    {
        public string Emoji { get; set; }
        public string Username { get; set; }
        public Color FriendIndicator { get; set; }
        public bool NotFriends { get; set; }
        public bool AreFriends { get; set; }
        public string UserColor { get; set; }
        public int friendship { get; set; }
        public string Id { get; set; }
        ColorHandler ch;
        public FrontClubMember(Account clubMember, int friendship)
        {
            this.friendship = friendship;
            ch = new ColorHandler();
            Id = clubMember.Id;
            this.Emoji = clubMember.Emoji;
            this.Username = clubMember.Username;
            NotFriends = false;
            AreFriends = !NotFriends;
            UserColor = clubMember.Color;
            FriendIndicator = ch.fromStringToColor("yellow");

            switch (this.friendship)
            {
                case 0:
                    NotFriends = true;
                    AreFriends = false;
                    break;
                case 1:
                    FriendIndicator = ch.fromStringToColor("yellow");
                    break;
                case 2:
                    FriendIndicator = ch.fromStringToColor("green");
                    break;
                default:

                    break;
            }

        }

    }
}
