using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using CloudClubv1._2_;
using Xamarin.Forms;

namespace FrontEnd
{
    class CommentViewCell : ViewCell
    {

        ColorHandler ch;
        TapGestureRecognizer dropletTGR;
        int dropletPressedCount;
        StackLayout decidingButtons;
        Label lVoted;
        TapGestureRecognizer reportUserTgr;
        public CommentViewCell()
        {
            dropletPressedCount = 0;
            dropletTGR = new TapGestureRecognizer();
            dropletTGR.Tapped += DropletTGR_Tapped;
            ch = new ColorHandler();
            reportUserTgr = new TapGestureRecognizer();
            reportUserTgr.Tapped += ReportUserTgr_Tapped;
            Image userEmoji = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Scale = 1,
                HeightRequest = 30

            };
            userEmoji.SetBinding(Image.SourceProperty, "UserEmoji");
            Label lUserId = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center

            };
            lUserId.SetBinding(Label.TextProperty, "AuthorUsername");
            lUserId.SetBinding(Label.TextColorProperty, "AuthorAccountColor", converter: new ColorConverter());
            Label lDropletNumber = new Label
            {
                TextColor = ch.fromStringToColor("black"),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center

            };
            lDropletNumber.SetBinding(Label.TextProperty, "NumDroplets");

            Image dropletImage = new Image
            {
                Source = FileImageSource.FromFile("DropletFull_WhiteB.png"),
                Aspect = Aspect.AspectFit,
                HeightRequest = 25,

                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center

            };
            dropletImage.GestureRecognizers.Add(dropletTGR);

            Label lCommentText = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = ch.fromStringToColor("black"),


            };
            lCommentText.SetBinding(Label.TextProperty, "Text");

            Label lReportUser = new Label
            {
                Text = "Report User",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = ch.fromStringToColor("gray"),
                HorizontalOptions = LayoutOptions.Center
            };
            lReportUser.SetBinding(Label.IsVisibleProperty, "ShowReport");
            lReportUser.GestureRecognizers.Add(reportUserTgr);

            //lCommentText.SetBinding(Label.TextColorProperty, "TextColor", converter: new ColorConverter());
            StackLayout headerLayout = new StackLayout
            {
                Children =
                {
                    userEmoji,
                    lUserId,
                    lDropletNumber,
                    dropletImage
                },
                Orientation = StackOrientation.Horizontal,
                Spacing = 11,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill
            };

            var commentSLayout = new StackLayout
            {
                Children =
               {
                   headerLayout,
                   new StackLayout
                   {
                       Children =
                       {
                           lCommentText,
                           lReportUser
                       },
                       Spacing = 6,
                       Padding = new Thickness(5,0,0,0)

                   }
                },
                BackgroundColor = ch.fromStringToColor("white"),
                Spacing = 11,
                Padding = new Thickness(12, 10, 10, 10),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand

            };
            commentSLayout.SetBinding(StackLayout.IsVisibleProperty, "ClubRequestBool", converter: new InvertBoolConverter());

            Label clubRequestHeader = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = ch.fromStringToColor("yellow")

            };
            clubRequestHeader.SetBinding(Label.TextProperty, "ClubRequestUsername");
            Label clubRequestText = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = ch.fromStringToColor("black"),
            };
            clubRequestText.SetBinding(Label.TextProperty, "ClubRequestText");

            var bReject = new Button
            {
                BackgroundColor = ch.fromStringToColor("red"),
                Text = "Reject",
                TextColor = ch.fromStringToColor("white"),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = 16,


            };
            bReject.SetBinding(Button.IsEnabledProperty, "IsMember");
            bReject.Clicked += BReject_Clicked;

            var bAccept = new Button
            {
                BackgroundColor = ch.fromStringToColor("green"),
                Text = "Accept",
                TextColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = 16
            };
            bAccept.SetBinding(Button.IsEnabledProperty, "IsMember");
            bAccept.Clicked += BAccept_Clicked;
            lVoted = new Label
            {
                Text = "Thanks for voting!",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = ch.fromStringToColor("black"),
                IsVisible = false
            };
            decidingButtons = new StackLayout
            {
                Children =
                {
                    bReject,
                    bAccept
                },
                Orientation = StackOrientation.Horizontal,
                Spacing = 5,
            };



            var clubRequestSLayout = new StackLayout
            {
                Children =
                {
                    clubRequestHeader,
                    clubRequestText,
                    decidingButtons,
                    lVoted
                },

                BackgroundColor = ch.fromStringToColor("white"),
                Spacing = 11,
                Padding = new Thickness(12, 10, 10, 10),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            clubRequestSLayout.SetBinding(StackLayout.IsVisibleProperty, "ClubRequestBool");

            View = new StackLayout
            {
                Children =
                {
                    commentSLayout,
                    clubRequestSLayout
                }
            };

        }

        private void ReportUserTgr_Tapped(object sender, EventArgs e)
        {
            var comment = (FrontComment)BindingContext;
            MessagingCenter.Send<CommentViewCell, FrontComment>(this, "hi", comment);


        }

        private async void BReject_Clicked(object sender, EventArgs e)
        {

            var frontComment = (FrontComment)BindingContext;

            await App.dbWrapper.DeclineClubRequest(frontComment.ClubRequestInstance.Id);
            decidingButtons.IsVisible = false;
            lVoted.IsVisible = true;


        }

        private async void BAccept_Clicked(object sender, EventArgs e)
        {
            var frontComment = (FrontComment)BindingContext;
            var isMember = await App.dbWrapper.IsClubMember(frontComment.ClubId, App.dbWrapper.GetUser().Id);

            await App.dbWrapper.AcceptClubRequest(frontComment.ClubRequestInstance.Id);
            decidingButtons.IsVisible = false;
            lVoted.IsVisible = true;
        }

        private async void DropletTGR_Tapped(object sender, EventArgs e)
        {
            dropletPressedCount++;
            var thisComment = (FrontComment)BindingContext;

            await App.dbWrapper.RateComment(thisComment.Id);

        }
        private int getCustomCellHeight(string commentText)
        {
            double heightRequest = 30;
            // assume a height of 10:1 height to line ratio where one line has 50 chars
            heightRequest = commentText.Length / 5;


            return (int)heightRequest;
        }
    }
}
