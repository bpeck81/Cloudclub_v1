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
    public class MyClubsPage : ContentPage
    {
        List<FrontMyClub> frontMyClubList;
        public string title = "Subscriptions";
        ColorHandler ch;
        public MyClubsPage(List<Club> memberClubList)
        {

            ch = new ColorHandler();
            frontMyClubList = new List<FrontMyClub>();
            generateDisplayList(memberClubList);

            ListView listView = new ListView
            {
                ItemsSource = frontMyClubList,
                ItemTemplate = new DataTemplate(typeof(MyClubViewCell))


            };

            ScrollView clubScroll = new ScrollView
            {
                Content = listView,
                Orientation = ScrollOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White
            };
            if (frontMyClubList.Count > 0) Content = clubScroll;
            else
            {
                Button bNewClub = new Button
                {
                    Text = "+",
                    FontSize = 40,
                    HeightRequest = 70,
                    WidthRequest = 70,
                    BorderRadius = 200,
                    BackgroundColor = ch.fromStringToColor("purple"),
                    TextColor = ch.fromStringToColor("white"),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,

                };
                bNewClub.Clicked += (object sender, EventArgs e) =>
                {
                    Navigation.PushAsync(new CreateClubPage());
                };
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = "You aren't in any Clubs!",
                            FontAttributes = FontAttributes.Bold,
                            TextColor= ch.fromStringToColor("red"),
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,

                            FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
                        },
                          new Label
                        {
                            Text = "Swipe right to explore and find interesting clubs!",
                            FontAttributes = FontAttributes.Bold,
                            TextColor= ch.fromStringToColor("gray"),
                            XAlign = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
                        },
                          new Label
                        {
                            Text = "OR",
                            FontAttributes = FontAttributes.Bold,
                            TextColor= ch.fromStringToColor("gray"),
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,

                            FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
                        },
                          new Label
                        {
                            Text = "Create a new club!",
                            FontAttributes = FontAttributes.Bold,
                            TextColor= ch.fromStringToColor("gray"),
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
                        },
                          bNewClub


                    },
                    Spacing = 20,
                    BackgroundColor = ch.fromStringToColor("white")

                };
            }
        }
        private void generateDisplayList(List<Club> clubList)
        {
            for (int i = 0; i < clubList.Count; i++)
            {
                frontMyClubList.Add(new FrontMyClub(clubList[i]));
            }


        }

    }
}
