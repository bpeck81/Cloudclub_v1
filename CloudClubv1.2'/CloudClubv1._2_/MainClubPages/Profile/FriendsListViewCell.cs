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
                HorizontalOptions = LayoutOptions.Center
            };
            userEmoji.SetBinding(Image.SourceProperty, "UserEmoji");
            var lUserName = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            lUserName.SetBinding(Label.TextProperty, "Username");
            lUserName.SetBinding(Label.TextColorProperty, "Color", converter: new ColorConverter());
            Button friendshipIndicator = new Button // TODO: custom renderer
            {
                HeightRequest = 50,
                WidthRequest = 50,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
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
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
        }
    }
}
