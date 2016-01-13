using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Backend;
using CloudClubv1._2_;

using Xamarin.Forms;

namespace FrontEnd
{
    public class FriendClubListPageViewCell : ViewCell
    {
        ColorHandler ch;
        public FriendClubListPageViewCell()
        {
            ch = new ColorHandler();

            var lClubName = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            lClubName.SetBinding(Label.TextProperty, "Title");
            lClubName.SetBinding(Label.TextColorProperty, "clubColor");

            var bMutualIndiicator = new Button
            {
                IsEnabled = false,
                BackgroundColor =  ch.fromStringToColor("green"),
                BorderRadius = 5,
                //WidthRequest = 30,
                Text = "Mutual",
                TextColor = ch.fromStringToColor("white")                
            };
            bMutualIndiicator.SetBinding(Button.IsVisibleProperty, "mutualBool");
            var bJoinIndicator = new Button
            {
                BackgroundColor = ch.fromStringToColor("gray"),
                TextColor = ch.fromStringToColor("white"),
                BorderRadius = 5,
               // WidthRequest = 30,
                Text = "Join",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bJoinIndicator.SetBinding(Button.IsVisibleProperty, "joinBool");
            bJoinIndicator.Clicked += async (sender, args) =>
            {
                FrontFriendClub ffc = (FrontFriendClub)BindingContext;
                
                var club  = await App.dbWrapper.GetClub(ffc.Id);
                if (club.Exclusive)
                {
                    await App.dbWrapper.CreateClubRequest("Please let me join your club!", ffc.Id);

                    bJoinIndicator.Text = "Request Sent";
                    bJoinIndicator.BackgroundColor = ch.fromStringToColor("gray");
                    bJoinIndicator.IsEnabled = false;
                }
                else
                {
                    await App.dbWrapper.JoinClub(club.Id);
                    bJoinIndicator.IsVisible = false;
                    bMutualIndiicator.IsVisible = true;
                    MessagingCenter.Send<FriendClubListPageViewCell>(this, "Refresh Page");
                }

            };

            var brequestJoinIndicator = new Button
            {
                BackgroundColor = ch.fromStringToColor("gray"),
                TextColor = ch.fromStringToColor("white"),
                BorderRadius = 5,
                Text = "Request Sent",
                IsEnabled = false,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
               // WidthRequest = 30
            };
            brequestJoinIndicator.SetBinding(Button.IsVisibleProperty, "requestJoinBool");
          /*  brequestJoinIndicator.Clicked += async (sender, args) =>
            {
                FrontFriendClub ffc = (FrontFriendClub)sender;
                await App.dbWrapper.CreateClubRequest("Please let me join your club!",ffc.Id);
                brequestJoinIndicator.Text = "Request Sent";
                brequestJoinIndicator.BackgroundColor = ch.fromStringToColor("gray");
                brequestJoinIndicator.IsEnabled = false;


            };

            var bpendingJoinRequest = new Button
            {
                BackgroundColor = ch.fromStringToColor("gray"),
                TextColor = ch.fromStringToColor("white"),
                BorderRadius = 5,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
               // WidthRequest = 50,
                Text = "Pending Request",
                IsEnabled = false
            };
            bpendingJoinRequest.SetBinding(Button.IsVisibleProperty, "pendingRequest");*/

            this.View = new StackLayout
            {
                Children =
                {
                    lClubName,
                    bMutualIndiicator,
                    bJoinIndicator,
                    brequestJoinIndicator,
                  //  bpendingJoinRequest
                },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(10,10,10,10),
                            

            };

            
        }
    }
}
