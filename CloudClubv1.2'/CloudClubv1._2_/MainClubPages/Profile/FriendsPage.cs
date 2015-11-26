using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using Backend;
using CloudClubv1._2_;
using System.Collections.ObjectModel;

namespace FrontEnd
{
    public class FriendsPage : ContentPage
    {
        List<FrontFriends> friendsList;
        ObservableCollection<FrontFriends> displayedFriends;
        ColorHandler ch;
        Entry searchBar;
        public FriendsPage(List<FrontFriends> friendsList)
        {
            this.friendsList = friendsList;
            displayedFriends = new ObservableCollection<FrontFriends>();
            for(int i =0; i<friendsList.Count; i++)
            {
                displayedFriends.Add(friendsList[i]);
            }
            Title = "Friends";
            ch = new ColorHandler();
            searchBar = new Entry
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
            var bar = (Entry)sender;
            for (int i = displayedFriends.Count - 1; i >= 0; i--)
            {

                displayedFriends.Remove(displayedFriends[i]);
            }// displayedFriends = searchedList;




                for (int i = 0; i < friendsList.Count; i++)
                {
                    if (friendsList[i].Username.Contains(bar.Text))
                    {
                        searchedList.Add(friendsList[i]);
                        System.Diagnostics.Debug.WriteLine(friendsList[i].Username);
                    }
                }
                if(searchedList.Count ==0)
            {
                for (int i = 0; i < friendsList.Count; i++)
                {
                    displayedFriends.Add(friendsList[i]);
                }

            }
            else
            {
                for (int i = 0; i < searchedList.Count; i++)
                {
                    displayedFriends.Add(searchedList[i]);
                    System.Diagnostics.Debug.WriteLine(searchedList[i].Username);

                }
            }

        

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
