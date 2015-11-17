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

        public FrontMyClub(Club club)
        {
            this.Id = club.Id;
            ColorHandler ch = new ColorHandler();
            clubColor = club.Color;
            recentText = "Most recent text...";
            Title = club.Title;
            founderId = club.FounderId;
            starNumber = club.GetRating();


        }
    }
}
