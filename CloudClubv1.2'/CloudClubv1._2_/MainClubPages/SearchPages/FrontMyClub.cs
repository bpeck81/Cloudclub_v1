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
    public class FrontMyClub : ParentFrontClub
    {
        public string recentText { get; set; }

        public FrontMyClub(Club club, string recentText)
        {
            this.Id = club.Id;
            ColorHandler ch = new ColorHandler();
            this.recentText = recentText;
            clubColor = club.Color;
            Title = club.Title;
            founderId = club.FounderId;
            starNumber = club.GetRating();


        }
    }
}
