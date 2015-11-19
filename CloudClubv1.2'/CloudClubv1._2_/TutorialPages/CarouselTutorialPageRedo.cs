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
    class CarouselTutorialPageRedo : CarouselPage
    {
        ColorHandler ch;
        public CarouselTutorialPageRedo()
        {
            ch = new ColorHandler();
            this.BackgroundColor = ch.fromStringToColor("purple");
            this.Title = "Tutorial";
            NavigationPage.SetHasNavigationBar(this, false);
            this.Children.Add(new Tutorial1Page());
            this.Children.Add(new TutorialPage5());
            this.Children.Add(new Tutorial2Page());
            this.Children.Add(new Tutorial3Page());
            this.Children.Add(new Tutorial4PageRedo());
        }
    }
}
