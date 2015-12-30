using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

using Backend;
using CloudClubv1._2_;


using Xamarin.Forms;

namespace FrontEnd
{
    public class ClubSearchViewCell : MyViewCell
    {
        static int clubStatus;
        //  Club club;
        static string clubText;
        Label header;
        Button bRequestJoin, bPendingRequest;
        Label clubTextLabel;
        Label activityTimeLabel;
        Label joinRequestLabel;
        Image star1, star2, star3, star4, star5, iFlag;
        ColorHandler ch;
        StackLayout starStack, headerLayout;
        TapGestureRecognizer flagTgr;

        public ClubSearchViewCell()
        {

            flagTgr = new TapGestureRecognizer();
            flagTgr.Tapped += FlagTgr_Tapped;

            ch = new ColorHandler();

            header = new Label
            {
           //     BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.Center
            };
            header.SetBinding(Label.TextProperty, "Title");
            header.SetBinding(Label.TextColorProperty, "clubColor", converter: new ColorConverter());
            headerLayout = new StackLayout
            {
                Children =
                {
                    header
                },
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions= LayoutOptions.Center,
                Padding = new Thickness(30,0,0,0)
            };


            iFlag = new Image
            {
                Source = FileImageSource.FromFile("reportflag.png"),
                Aspect = Aspect.AspectFit,
                WidthRequest = 20,
                HeightRequest = 20,
                //  BackgroundColor = ch.fromStringToColor("red"),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,

            };
            iFlag.GestureRecognizers.Add(flagTgr);

            bRequestJoin = new Button
            {
                BorderRadius = 5,
                Text = "Join",
                FontSize = 13,
                WidthRequest = 170,
                HeightRequest = 34,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White
            };
            bRequestJoin.SetBinding(Button.BackgroundColorProperty, "clubColor", converter: new ColorConverter());
            bRequestJoin.SetBinding(Button.IsVisibleProperty, "isNotMemberNoPending");
            //  bRequestJoin.SetBinding(Button.IsEnabledProperty, "isMember", converter: new InvertBoolConverter());
            bRequestJoin.Clicked += BRequestJoin_Clicked;

            bPendingRequest = new Button
            {
                BorderRadius = 5,
                Text = "Pending Request",
                FontSize = 13,
                WidthRequest = 170,

                HeightRequest = 34,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = ch.fromStringToColor("white"),
                BackgroundColor = ch.fromStringToColor("gray"),
                IsEnabled = false
            };
            bPendingRequest.SetBinding(Button.IsVisibleProperty, "pendingInvite");


            clubTextLabel = new Label
            {
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center

            };
            clubTextLabel.SetBinding(Label.TextProperty, "mostRecentLine");

            clubTextLabel.SetBinding(Label.IsVisibleProperty, "isMember");


            activityTimeLabel = new Label
            {
                TextColor = ch.fromStringToColor("gray"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand

            };
            activityTimeLabel.SetBinding(Label.TextProperty, "timeSinceActivity");
            Image star1 = new Image { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
            Image star2 = new Image { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
            Image star3 = new Image { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
            Image star4 = new Image { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
            Image star5 = new Image { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
            /*
                        Image star1 = new Image { Aspect = Aspect.AspectFit };
                        Image star2 = new Image { Aspect = Aspect.AspectFit };
                        Image star3 = new Image { Aspect = Aspect.AspectFit };
                        Image star4 = new Image { Aspect = Aspect.AspectFit };
                        Image star5 = new Image { Aspect = Aspect.AspectFit };

                */
            star1.SetBinding(Image.SourceProperty, "star1");
            star2.SetBinding(Image.SourceProperty, "star2");
            star3.SetBinding(Image.SourceProperty, "star3");
            star4.SetBinding(Image.SourceProperty, "star4");
            star5.SetBinding(Image.SourceProperty, "star5");
            star1.Scale = .7;
            star2.Scale = .7;
            star3.Scale = .7;
            star4.Scale = .7;
            star5.Scale = .7;


            starStack = new StackLayout
            {
                Children =
                {
                    star1,
                    star2,
                    star3,
                    star4,
                    star5
                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 20,
                Padding = new Thickness(10, 0, 10, 0)
            };

            StackLayout cellWrapper = new StackLayout
            {
                Children =
                    {
                        new StackLayout
                        {
                            Children =
                            {
                                headerLayout,
                                iFlag
                            },
                            Padding = new Thickness(10,5,10,0),
                            Orientation = StackOrientation.Horizontal

                        },
                        clubTextLabel,
                        bRequestJoin,
                        bPendingRequest,
                        starStack,
                        activityTimeLabel
                     },
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
          //     BackgroundColor = Color.White,
                Spacing = 9f

            };
            View = cellWrapper;
        }

        private async void FlagTgr_Tapped(object sender, EventArgs e)
        {
            var club = (FrontClub)BindingContext;
            MessagingCenter.Send<ClubSearchViewCell, string>(this, "Hi", club.Id);


        }


        private void BReportNo_Clicked(object sender, EventArgs e)
        {
        }

        private async void BReportYes_Clicked(object sender, EventArgs e)
        {
            var club = (FrontClub)BindingContext;
            await App.dbWrapper.CreateClubReport(club.Id, App.dbWrapper.GetUser().Id);
        }

        private async void BRequestJoin_Clicked(object sender, EventArgs e)
        {

            var thisClub = (FrontClub)BindingContext;
            if (thisClub.exclusive)
            {
                await App.dbWrapper.CreateClubRequest("Please let me join your club!", thisClub.Id);
                bPendingRequest.IsVisible = true;
            }
            else
            {
                await App.dbWrapper.JoinClub(thisClub.Id);
                clubTextLabel.IsVisible = true;

            }

            var btn = sender as Button;

            btn.IsVisible = false;
            btn.IsEnabled = false;


        }

    }
}

