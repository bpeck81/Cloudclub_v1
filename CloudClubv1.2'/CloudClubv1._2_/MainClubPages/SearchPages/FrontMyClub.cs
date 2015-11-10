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
    class FrontMyClub
    {
        Color clubColor { get; set; }
        string Title { get; set; }
        string recentText { get; set; }

        public FrontMyClub(Club club)
        {
            ColorHandler ch = new ColorHandler();
            clubColor = ch.fromStringToColor(club.Color);
            recentText = "most recent text";
            Title = club.Title;


        }
    }
}
