using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Backend;

using Xamarin.Forms;

namespace FrontEnd
{
    public class MyClubViewCell : ViewCell
    {
        ColorHandler ch;
        public MyClubViewCell()
        {
            ch = new ColorHandler();
            Label headerL = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
            headerL.SetBinding(Label.TextProperty, "Title");
            headerL.SetBinding(Label.TextColorProperty, "clubColor", converter: new ColorConverter());


            Label recentTextL = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = ch.fromStringToColor("black")
            };
            recentTextL.SetBinding(Label.TextProperty, "recentText");
            View = new StackLayout
            {
                Children =
                {
                    headerL,
                    recentTextL
                },
                Padding = new Thickness(0, 10, 0, 10),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
        }
    }
}
