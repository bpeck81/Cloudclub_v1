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
            //   headerL.SetBinding(Label.TextColorProperty, "clubColor");


            Label recentTextL = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.Black
            };
            recentTextL.SetBinding(Label.TextProperty, "recentText");
            View = new StackLayout
            {
                Children =
                {
                    headerL,
                    recentTextL
                },
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
        }
    }
}
