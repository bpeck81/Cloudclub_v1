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
    public class inviteFriendsViewCell : ViewCell
    {
        ColorHandler ch;
        Image iEmoji;
        Label lUsername;
        Button bAddFriend, bFriendsIndicator;
        public inviteFriendsViewCell()
        {
            ch = new ColorHandler();
            updateView();
        }
        private void updateView()
        {
            iEmoji = new Image
            {
                Aspect = Aspect.AspectFit,
                WidthRequest = 75,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            iEmoji.SetBinding(Image.SourceProperty, "Emoji");

            lUsername = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.Center
            };
            lUsername.SetBinding(Label.TextProperty, "Username");
            lUsername.SetBinding(Label.TextColorProperty, "UserColor", converter: new ColorConverter());

            bAddFriend = new Button
            {
                Text = "+",
                FontAttributes = FontAttributes.Bold,
                TextColor = ch.fromStringToColor("gray"),
                BackgroundColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bAddFriend.Clicked += BAddFriend_Clicked;

            bFriendsIndicator = new Button
            {
                BackgroundColor = ch.fromStringToColor("yellow"),
                BorderRadius = 100,
                HeightRequest = 20,
                WidthRequest = 20,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
                IsVisible = false
            };

            View = new StackLayout
            {
                Children ={
                    iEmoji,
                    lUsername,
                    bAddFriend,
                    bFriendsIndicator
                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 0, 10, 0),
                BackgroundColor = ch.fromStringToColor("white")
            };
        }
        private async void BAddFriend_Clicked(object sender, EventArgs e)
        {
            FrontClubMember clubMember = (FrontClubMember)BindingContext;
            var btn = sender as Button;

            clubMember.friendship = await App.dbWrapper.GetFriendship(clubMember.Id);
            bAddFriend.IsVisible = false;
            bFriendsIndicator.IsVisible = true;
            updateView();

        }
    }
}
