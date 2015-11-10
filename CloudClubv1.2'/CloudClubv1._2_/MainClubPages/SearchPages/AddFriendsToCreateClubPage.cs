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
    public class AddFriendsToCreateClubPage : ContentPage
    {
        ColorHandler ch;
        public AddFriendsToCreateClubPage(List<FrontFriends> friendsList)
        {
            ch = new ColorHandler();
            Title = "Invite Friends";
            
            Entry eSearchFriends = new Entry
            {
                Placeholder = "Search",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = ch.fromStringToColor("white"),
                TextColor= ch.fromStringToColor("black")
            };

            ListView listView = new ListView
            {
                ItemsSource = friendsList,
                ItemTemplate = new DataTemplate(typeof(inviteFriendsViewCell)),
                RowHeight = 60,
                BackgroundColor = ch.fromStringToColor("white"),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            if (friendsList.Count != 0)
            {
                Content = new StackLayout
                {
                    Children =
                {
                    listView
                },
                    BackgroundColor = ch.fromStringToColor("white"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

            }
            else
            {
                Content = new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = "You Currently Have No Friends :(",
                            TextColor = ch.fromStringToColor("black"),
                            FontSize = 36,
                            FontAttributes = FontAttributes.Bold,

                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        }
                    },
                    BackgroundColor = ch.fromStringToColor("white"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
            }

        }
    }
}
