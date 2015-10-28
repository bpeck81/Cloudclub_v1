﻿using System;
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
    public class ClubSearchViewCell : ViewCell
    {
        static int clubStatus;
      //  Club club;
        static string clubText;
        Label header;
        Button bRequestJoin;
        Label clubTextLabel;
        Label activityTimeLabel;
        Label joinRequestLabel;
        Image star1,star2,star3,star4,star5;
        ColorHandler ch;
        StackLayout starStack;

        public ClubSearchViewCell()
        {
            clubText = "Most Recent Line of text from club";
            bool inClub = true;
            ch = new ColorHandler();

            header = new Label
            {
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize =Device.GetNamedSize(NamedSize.Large,typeof(Label)),
                VerticalOptions = LayoutOptions.Center
            };
            header.SetBinding(Label.TextProperty, "Title");
            header.SetBinding(Label.TextColorProperty, "clubColor", converter: new ColorConverter());

            bRequestJoin = new Button
            {
                BorderRadius = 5,
                Text = "Join",
                FontSize= 13,
                WidthRequest = 170,
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White
            };
            bRequestJoin.SetBinding(Button.BackgroundColorProperty, "clubColor", converter: new ColorConverter());
            bRequestJoin.SetBinding(Button.IsVisibleProperty, "isNotMember");
            bRequestJoin.SetBinding(Button.IsEnabledProperty, "isNotMember");

            bRequestJoin.Clicked += BRequestJoin_Clicked;
            clubTextLabel = new Label
            {
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center

            };
            clubTextLabel.SetBinding(Label.IsVisibleProperty, "isMember");
            clubTextLabel.SetBinding(Label.TextProperty, "mostRecentLine");

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
                Padding = new Thickness(10,0,10,0)
            };

            StackLayout cellWrapper = new StackLayout
            {
                Children =
                    {

                        header,
                        clubTextLabel,
                        bRequestJoin,
                        starStack,
                        activityTimeLabel
                     },
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.White,
                Spacing = 9f

            };
            View = cellWrapper;
        }



        private async void BRequestJoin_Clicked(object sender, EventArgs e)
        {
            var thisClub = (FrontClub)BindingContext;
            System.Diagnostics.Debug.WriteLine(thisClub.Id.ToString());
             await App.dbWrapper.JoinClub(thisClub.Id);
            var btn = sender as Button;
            btn.Text = "Request Sent";
            Color prevColor = btn.BackgroundColor; //= ch.fromStringToColor("grayPressed");
            int r =(int) prevColor.R;int g =(int) prevColor.G; int b = (int)prevColor.B;
            if (prevColor.R + 20 <= 250) r =(int) prevColor.R + 20;
            if (prevColor.G + 20 <= 250) g = (int)prevColor.G + 20;

            if (prevColor.B + 20 <= 250) b =(int) prevColor.B + 20;

            btn.BackgroundColor = ch.fromStringToColor("lightGray");
            btn.TextColor = ch.fromStringToColor("gray");
            btn.IsEnabled = false;

        }

        public static async void getClubStatus(Club club)
        {

            clubStatus = await App.dbWrapper.JoinClub(club.Id);// get backend method to see if in club already
                                                               // add logic to return true
                                                               // return false;
        }
        public static async void getClubText(Club club)
        {
            
        }
       
    }
}

