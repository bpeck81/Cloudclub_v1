using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace FrontEnd
{ 
    public class TutorialPage5 : ContentPage
    {
        public TutorialPage5()
        {
        ColorHandler ch = new ColorHandler();
            BackgroundColor = ch.fromStringToColor("purple");

            Label header = new Label
        {
            
            Text = "How It Works",
            TextColor = ch.fromStringToColor("white"),
            FontAttributes = FontAttributes.Bold,
            FontSize = 42,
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            VerticalOptions = LayoutOptions.CenterAndExpand
        };
            var cloudImages = new Image
            {
                Source = FileImageSource.FromFile("Tutorial_Clouds.png"),
                Aspect = Aspect.AspectFit,
                WidthRequest = 250,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var lChooseACloud = new Label
            {
                Text = "Choose A Cloud",
                FontSize = 42,
                TextColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            var lText = new Label
            {
                Text = "Clouds hold group chat rooms called clubs. When you enter a State or University you unlock the ability to join that location's Cloud",
                TextColor = ch.fromStringToColor("white"),
                FontAttributes = FontAttributes.Bold,
                XAlign = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand

            };
            Content = new StackLayout
            {
                Children =
                {
                    header,
                    cloudImages,
                    lChooseACloud,
                    lText
                },
                Spacing = 20,
                Padding = new Thickness(10, 20, 10, 20)
            };


        }
    }
}
