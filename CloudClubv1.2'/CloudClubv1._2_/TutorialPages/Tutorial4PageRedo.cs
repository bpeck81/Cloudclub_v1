using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace FrontEnd
{
    public class Tutorial4PageRedo : ContentPage
    {
        ColorHandler ch;
        public Tutorial4PageRedo()
        {
            ch = new ColorHandler();
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // page 4 of tutorial
            Label topHeader = new Label
            {
                Text = "How It Works",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 42,
                TextColor = Color.White,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                BackgroundColor = Color.FromRgb(210, 61, 235)

            };
            Image cloudImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Tutorial_Competition.png"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Scale = .75
            };


            Label lowerHeader = new Label
            {
                Text = "Achieve Greatness",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 36,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                TextColor = Color.White
            };
            Label informerLabel = new Label
            {
                Text = "Earn rewards for creating popular clubs and chats! Rise through the ranks to become a Cloudclub legend!",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White
            };
            var bReturn = new Button
            {
                Text = "Finish",
                TextColor = ch.fromStringToColor("white"),
                BackgroundColor = ch.fromStringToColor("green"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center

            };
            bReturn.Clicked += (sender, args) =>
            {
                Navigation.PopToRootAsync();
            };


            Content = new StackLayout
            {
                Children = {
                    topHeader,
                    cloudImage,
                    lowerHeader,
                    informerLabel,
                    bReturn

                },
                BackgroundColor = Color.FromRgb(210, 61, 235),
                Spacing = 30,
                Padding = new Thickness(20, 0, 20, 20)


            };


        }
    }
}
