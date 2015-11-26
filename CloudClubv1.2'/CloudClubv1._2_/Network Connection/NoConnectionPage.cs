using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using Backend;
using CloudClubv1._2_;

namespace FrontEnd
{
    public class NoConnectionPage : ContentPage
    {
        ColorHandler ch;
        public NoConnectionPage()
        {
            ch = new ColorHandler();
            
            var lNoConnection = new Label
            {
                Text = "You are not connected to a network.",
                FontSize = 42,
                TextColor = ch.fromStringToColor("black"),
                XAlign =TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Content = new StackLayout
            {
                Children =
                {
                    lNoConnection
                },
                BackgroundColor = ch.fromStringToColor("lightGray")
            };

        }
    }
}
