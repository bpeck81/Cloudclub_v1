using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace FrontEnd
{
    public class Tutorial3Page : ContentPage
    {
        ColorHandler ch;
        public Tutorial3Page()
        {
            ch = new ColorHandler();
            BackgroundColor = ch.fromStringToColor("purple");

            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
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
                Source = ImageSource.FromFile("page3Tutorial.png"),
                HeightRequest = 175,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Label lowerHeader = new Label
            {
                Text = "Interact Dynamically",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = 36,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                TextColor = Color.White
            };
            Label informerLabel = new Label
            {
                Text = "Stay active! Clubs are deleted after 24 hours of inactivity",
                XAlign = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White
            };


            
            StackLayout sLayout = new StackLayout
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


            Content = sLayout;
        }
    }
}
