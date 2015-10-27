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
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = "You aren't in any Clubs!",
                            FontAttributes = FontAttributes.Bold,
                            TextColor= ch.fromStringToColor("black"),
                            FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
                        }
                    },
                    BackgroundColor = ch.fromStringToColor("lightGray")

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
