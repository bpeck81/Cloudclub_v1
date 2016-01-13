using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudClubv1._2_;
using Xamarin.Forms;
using Backend;
namespace FrontEnd
{
    public class FrontFriendClub
    {
        public string Title { get; set; }
        public Color clubColor { get; set; }
        public bool mutualBool { get; set; }
        public bool joinBool { get; set; }
        public bool requestJoinBool { get; set; }
        public bool exclusive { get; set; }
        public string Id { get; set; }
        public bool pendingRequest { get; set; }
        public FrontFriendClub(Club club, bool mutual, bool joinBool, bool requestJoin, bool pendingRequest)
        {
            ColorHandler ch = new ColorHandler();
            Title = club.Title;
            clubColor = ch.fromStringToColor(club.Color);
            this.mutualBool = mutual;
            this.joinBool = joinBool;

            this.requestJoinBool = requestJoin;

           
            exclusive = club.Exclusive;
            this.Id = club.Id;
            this.pendingRequest = pendingRequest;
        }

    }
}
