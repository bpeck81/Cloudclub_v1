using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace FrontEnd
{
    public class Tutorial4Page : ContentPage
    {
        public Tutorial4Page()
        {
            ColorHandler ch = new ColorHandler();

            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // page 4 of tutorial
            Label topHeader = new Label
            {

                Text = "How It Works",
                TextColor = ch.fromStringToColor("white"),
                FontAttributes = FontAttributes.Bold,
                FontSize = 42,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            Image cloudImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Tutorial_Competition.png"),
                VerticalOptions = LayoutOptions.CenterAndExpand,

                Scale = .75,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };


            Label lowerHeader = new Label
            {
                Text = "Achieve Greatness",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = 36,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                TextColor = Color.White
            };
            Label informerLabel = new Label
            {
                Text = "Earn rewards for creating popular clubs and chats! Rise through the ranks to become a Cloudclub legend!",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White
            };


            Content= new StackLayout
            {
                Children = {
                    topHeader,
                    cloudImage,
                    lowerHeader,
                    informerLabel

                },
                BackgroundColor = Color.FromRgb(210, 61, 235),
                Spacing = 20,
                Padding = new Thickness(10, 20, 10, 20)


            };


        }
    }
}
