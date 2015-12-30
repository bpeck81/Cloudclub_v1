using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace FrontEnd
{
    public class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            ColorHandler ch = new ColorHandler();
            BackgroundColor = ch.fromStringToColor("purple");
            ActivityIndicator indicator = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsRunning = true
            };
            var lLoadContent = new Label
            {
                Text = "Loading Content",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = ch.fromStringToColor("white"),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                FontAttributes = FontAttributes.Bold
            };
            var cloudImage = new Image
            {
                Source = FileImageSource.FromFile("splashcloud.png"),
              //  Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            Content = new StackLayout
            {
                Children =
                {
                    indicator,
                    lLoadContent
                },
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
               
            };
        }
    }
}
