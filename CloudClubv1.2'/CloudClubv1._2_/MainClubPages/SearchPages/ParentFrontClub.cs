using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CloudClubv1._2_;
using Backend;
using System.ComponentModel;
namespace FrontEnd
{
    public class ParentFrontClub
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string founderId { get; set; }
        public int starNumber { get; set; }
        public string clubColor { get; set; }
        public string cloudId { get; set; }
    }
}
