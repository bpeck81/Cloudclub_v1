using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using Backend;

namespace FrontEnd
{
    public class FrontInviteToClubFriend
    {
        public string Title { get; set; }
        public Color clubColor { get; set; }
        public bool inviteBool { get; set; }
        public bool mutualClubBool { get; set; }
        public string Id { get; set; }
        public bool pendingInviteBool { get; set; }
        public string friendId;
        
        public FrontInviteToClubFriend(Club club, bool mutualClubBool, bool inviteBool, bool pendingInviteBool, string friendId)
        {
            ColorHandler ch = new ColorHandler();
            Id = club.Id;
            Title = club.Title;
            this.friendId = friendId;
            clubColor = ch.fromStringToColor(club.Color);
            this.mutualClubBool = mutualClubBool;
            this.inviteBool = inviteBool;
            this.pendingInviteBool = pendingInviteBool;
            if(pendingInviteBool == true)
            {
                inviteBool = false;
            }
                           


        }
    }
}
