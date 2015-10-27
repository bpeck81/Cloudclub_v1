using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace FrontEnd
{
    public class Tutorial2Page : ContentPage
    {
        public Tutorial2Page()
        {
            // page 2 of tutorial
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
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
                Source = ImageSource.FromFile("page2Tutorial.png"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            Label lowerHeader = new Label
            {
                Text = "Find & Join Clubs",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 36,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                TextColor = Color.White
            };
            Label informerLabel = new Label
            {
                Text = "Chat and build friendships with similar people in a care-free environment.",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
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
                Padding = new Thickness(20, 0, 20, 20)

            };


        Content =sLayout;
        }
    }
}
