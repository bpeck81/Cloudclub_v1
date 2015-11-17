using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using Xamarin.Forms;
using CloudClubv1._2_;

namespace FrontEnd
{
    public class ClubMemberViewCell : ViewCell
    {
        ColorHandler ch;
        Image iEmoji;
        Label lUsername;
        Button bAddFriend, bFriendsIndicator;
        TapGestureRecognizer nameTGR;
        public ClubMemberViewCell()
        {

            nameTGR = new TapGestureRecognizer();
            nameTGR.Tapped += NameTGR_Tapped;

            ch = new ColorHandler();
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
            lUsername.GestureRecognizers.Add(nameTGR);
            updateView();
        }


        private void updateView()
        {



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
            bAddFriend.SetBinding(Button.IsVisibleProperty, "NotFriends");
            bFriendsIndicator = new Button
            {
                BorderRadius = 100,
                HeightRequest = 20,
                WidthRequest = 20,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = ch.fromStringToColor("yellow")

            };
            bFriendsIndicator.SetBinding(Button.BackgroundColorProperty, "FriendIndicator");
            bFriendsIndicator.SetBinding(Button.IsVisibleProperty, "AreFriends");
            View = new StackLayout
            {
                Children ={
                    iEmoji,
                    lUsername,
                    bAddFriend,
                    new StackLayout
                    {
                        Children =
                        {
                            bFriendsIndicator
                        },
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(0,0,5,0)

                    }

                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 0, 10, 0),
                BackgroundColor = ch.fromStringToColor("white")
            };
        }
        private void NameTGR_Tapped(object sender, EventArgs e)
        {
            var name = (Label)sender;

            var user = (FrontClubMember)BindingContext;
            MessagingCenter.Send<ClubMemberViewCell, FrontClubMember>(this, "friendsProfilePage", user);
        }

        private async void BAddFriend_Clicked(object sender, EventArgs e)
        {
            FrontClubMember clubMember = (FrontClubMember)BindingContext;
            var btn = sender as Button;

            await App.dbWrapper.CreateFriendRequest(clubMember.Id);
            //clubMember.friendship = await App.dbWrapper.GetFriendship(clubMember.Id

            clubMember.NotFriends = false;
            clubMember.AreFriends = true;
            clubMember.FriendIndicator = ch.fromStringToColor("yellow");

            updateView();

        }
    }
}
