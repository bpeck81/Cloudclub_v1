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
        public Tutorial3Page()
        {
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // page 3 of tutorial
            Label topHeader = new Label
            {
                Text = "How It Works",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.End,
                FontAttributes = FontAttributes.Bold,
                FontSize = 42,
                TextColor = Color.White,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                BackgroundColor = Color.FromRgb(210, 61, 235)

            };

            Image cloudImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("TutorialImgs/page3Tutorial.png"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            Label lowerHeader = new Label
            {
                Text = "Interact Dynamically",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 36,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                TextColor = Color.White
            };
            Label informerLabel = new Label
            {
                Text = "Stay active! Clubs are deleted after 24 hours of inactivity",
                XAlign = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White
            };


            var spinningclock = new OpenGLView
            {
                HasRenderLoop = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            spinningclock.OnDisplay = r =>
            {
                
            };
            StackLayout sLayout = new StackLayout
            {
                Children = {
                    topHeader,
                    spinningclock,
                    lowerHeader,
                    informerLabel

                },
                BackgroundColor = Color.FromRgb(210, 61, 235),
                Spacing = 20,
                Padding = new Thickness(20, 0, 20, 20)


            };


            Content = sLayout;
        }
    }
}
