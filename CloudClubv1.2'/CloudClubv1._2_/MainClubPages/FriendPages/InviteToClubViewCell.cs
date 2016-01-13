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
    public class InviteToClubViewCell : ViewCell
    {
        ColorHandler ch;
        public InviteToClubViewCell()
        {
            ch = new ColorHandler();

            var lClubName = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            };

            lClubName.SetBinding(Label.TextProperty, "Title");
            lClubName.SetBinding(Label.TextColorProperty, "clubColor");

            var bMutualIndicator = new Button
            {
                IsEnabled = false,
                BackgroundColor = ch.fromStringToColor("green"),
                BorderRadius = 5,
                //WidthRequest = 30,
                Text = "Mutual",
                TextColor = ch.fromStringToColor("white")
            };
            bMutualIndicator.SetBinding(Button.IsVisibleProperty, "mutualClubBool");

            var bInvite = new Button
            {
                BackgroundColor = ch.fromStringToColor("white"),
                Text = "+",
                TextColor = ch.fromStringToColor("gray"),
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bInvite.SetBinding(Button.IsVisibleProperty, "inviteBool");
            bInvite.Clicked += async (sender, args) =>
             {
                 var item = (FrontInviteToClubFriend)BindingContext;
                 await App.dbWrapper.CreateInvite(item.Id, item.friendId);
                 bInvite.Text = "Invite Sent";
                 bInvite.IsEnabled = false;
                 bInvite.BackgroundColor = ch.fromStringToColor("gray");
                 bInvite.TextColor = ch.fromStringToColor("white");
             };
           
            var bpendingJoinRequest = new Button
            {
                BackgroundColor = ch.fromStringToColor("gray"),
                TextColor = ch.fromStringToColor("white"),
                BorderRadius = 5,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
                // WidthRequest = 50,
                Text = "Invite Sent",

                IsEnabled = false
            };
            bpendingJoinRequest.SetBinding(Button.IsVisibleProperty, "pendingInviteBool");

            this.View = new StackLayout
            {
                Children =
                {
                    lClubName,
                    bMutualIndicator,
                    bInvite,
                    bpendingJoinRequest
                },
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(10, 10, 10, 10),


            };


        }
    }
}
