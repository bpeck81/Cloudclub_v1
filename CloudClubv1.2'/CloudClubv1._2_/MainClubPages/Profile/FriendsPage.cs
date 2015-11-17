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
    public class FriendsPage : ContentPage
    {
        List<FrontFriends> friendsList;
        List<FrontFriends> displayedFriends;
        ColorHandler ch;
        Entry searchBar;
        public FriendsPage(List<FrontFriends> friendsList)
        {
            this.friendsList = friendsList;
            displayedFriends = friendsList;
            Title = "Friends";
            ch = new ColorHandler();
            Entry searchBar = new Entry
            {
                Placeholder = "Search",
                BackgroundColor = ch.fromStringToColor("white"),
                TextColor = ch.fromStringToColor("black")

            };
            searchBar.Completed += SearchBar_Completed;
            ListView listView = new ListView
            {
                ItemsSource = displayedFriends,
                ItemTemplate = new DataTemplate(typeof(FriendsListViewCell)),
                RowHeight = 75
            };
            listView.ItemSelected += ListView_ItemSelected;

            BackgroundColor = ch.fromStringToColor("lightGray");
            Content = new StackLayout
            {
                Children =
                {
                    searchBar,
                    listView
                }
            };
        }

        private void SearchBar_Completed(object sender, EventArgs e)
        {
            var searchedList = new List<FrontFriends>();
            for (int i = 0; i < friendsList.Count; i++)
            {
                if (friendsList[i].Username.Contains(searchBar.Text))
                {
                    searchedList.Add(friendsList[i]);
                }
            }
            displayedFriends = searchedList;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var frontFriend = (FrontFriends)e.SelectedItem;
            var friend = await App.dbWrapper.GetAccount(frontFriend.Id);
            var friendStatus = await App.dbWrapper.GetFriendship(frontFriend.Id);



            await Navigation.PushAsync(new FriendProfilePage(friend, friendStatus));
        }
    }
}
