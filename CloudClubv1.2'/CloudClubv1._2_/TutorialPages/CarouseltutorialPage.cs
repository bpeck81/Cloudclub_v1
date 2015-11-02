using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace FrontEnd
{
    public class CarouselTutorialPage : CarouselPage
    {
        ColorHandler ch;
        public CarouselTutorialPage()
        {
            ch = new ColorHandler();
            this.Title = "Tutorial";
            NavigationPage.SetHasNavigationBar(this, false);
            this.BackgroundColor = ch.fromStringToColor("purple");


            this.Children.Add(new Tutorial1Page());
            this.Children.Add(new Tutorial2Page());
            this.Children.Add(new Tutorial3Page());
            this.Children.Add(new Tutorial4Page());
            this.Children.Add(new SignUpPage());

        }
    }
}
