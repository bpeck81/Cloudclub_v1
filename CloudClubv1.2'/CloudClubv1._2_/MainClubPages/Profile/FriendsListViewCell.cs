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
    public class FriendsListViewCell : ViewCell
    {
        ColorHandler ch;
        public FriendsListViewCell()
        {
            ch = new ColorHandler();
            Image userEmoji = new Image
            {
                Aspect = Aspect.AspectFit,
                WidthRequest = 50,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            userEmoji.SetBinding(Image.SourceProperty, "Emoji");
            var lUserName = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 26
            };
            lUserName.SetBinding(Label.TextProperty, "Username");
            lUserName.SetBinding(Label.TextColorProperty, "Color", converter: new ColorConverter());
            Button friendshipIndicator = new Button // TODO: custom renderer
            {
                HeightRequest = 25,
                WidthRequest = 25,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            friendshipIndicator.SetBinding(Button.BackgroundColorProperty, "SharedClubIndicator");
            View = new StackLayout
            {
                Children =
                {
                    userEmoji,
                    lUserName,
                    friendshipIndicator
                },
                BackgroundColor = ch.fromStringToColor("white"),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(10,0,10,0)
            };
        }
    }
}
