using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using CloudClubv1._2_;
using Backend;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace FrontEnd
{
    public class ClubChatPage : ContentPage
    {

        public static ObservableCollection<FrontComment> CurrentCommentsList;
        ObservableCollection<FrontComment> commentsList;
        ColorHandler ch;
        Entry userEntry;
        ParentFrontClub club;
        bool isMember;
        ListView listView;
        public ClubChatPage(ParentFrontClub club, List<DBItem> chatList, List<Account> commentUsers, List<Account> requestUsersList, bool isMember)
        {
            this.isMember = isMember;
            this.club = club;
            ch = new ColorHandler();
            this.BackgroundColor = Color.Black;
            this.Title = club.Title;
            this.ToolbarItems.Add(new ToolbarItem
            {
                Icon = "Settings_Top.png",

                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => menuPopup())
            });
            this.commentsList = new ObservableCollection<FrontComment>();
            int clubRequestCount = 0;
            for (int i = 0; i < chatList.Count; i++)
            {
                if (chatList[i].GetType() == typeof(Comment))
                {
                    if (commentUsers[i] != null)
                    {
                        this.commentsList.Add(new FrontComment((Comment)chatList[i], commentUsers[i - clubRequestCount]));

                    }
                }
                else if (chatList[i].GetType() == typeof(ClubRequest))
                {
                    this.commentsList.Add(new FrontComment((ClubRequest)chatList[i], requestUsersList[clubRequestCount], this.isMember));
                    clubRequestCount++;

                }
            }
            CurrentCommentsList = this.commentsList;

            updatePage();



        }
        private void updatePage()
        {

            listView = new ListView
            {
                ItemsSource = CurrentCommentsList,
                ItemTemplate = new DataTemplate(typeof(CommentViewCell)),
                HasUnevenRows = true
            };
            listView.ItemTapped += ListView_ItemTapped;

            MessagingCenter.Subscribe<CommentViewCell, FrontComment>(this, "hi", async (sender, args) =>
            {
                var comment = (FrontComment)args;
                var answer = await DisplayAlert("Report User", "Do you really want to report " + comment.AuthorUsername + "?", "Yes", "No");
                if (answer)
                {
                    await App.dbWrapper.CreateBan(comment.AuthorId, comment.Id, App.dbWrapper.GetUser().Id);
                    comment.ShowReport = false;

                }

                updatePage();

            });


            userEntry = new Entry
            {
                BackgroundColor = ch.fromStringToColor("white"),
                TextColor = ch.fromStringToColor("black"),
                VerticalOptions = LayoutOptions.End
            };
            userEntry.Completed += UserEntry_Completed;
            Label lEmptyChat = new Label
            {
                Text = "There are no messages. Type below!",
                FontSize = 38,
                TextColor = ch.fromStringToColor("black"),
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.FillAndExpand

            };
            if (CurrentCommentsList.Count != 0)
            {
                Content = new StackLayout
                {
                    Children =
                {
                    listView,
                    userEntry
                },
                    BackgroundColor = ch.fromStringToColor("lightGray"),
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
                        lEmptyChat,
                        userEntry
                    },
                    BackgroundColor = ch.fromStringToColor("lightGray"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
            }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (FrontComment)e.Item;
            for (int i = 0; i < CurrentCommentsList.Count; i++)
            {
                if (CurrentCommentsList[i].Id == item.Id && CurrentCommentsList[i].ClubRequestBool == false)
                {
                    CurrentCommentsList[i].ShowReport = true;
                }
                else
                {
                    CurrentCommentsList[i].ShowReport = false;
                }
            }
            updatePage();

        }

        private async void UserEntry_Completed(object sender, EventArgs e)
        {
            if (userEntry.Text != "")
            {
                var joinClub = await App.dbWrapper.JoinClub(club.Id);
                var commentOutput = await App.dbWrapper.CreateComment(userEntry.Text, club.Id);
                //System.Diagnostics.Debug.WriteLine("OUTPUT: "+joinClub);
                userEntry.Text = "";
                updatePage();
            }


        }

        private async void menuPopup()
        {
            var tagsList = await App.dbWrapper.GetTags(club.Id);
            var usersList = await App.dbWrapper.GetClubMembers(club.Id);
            var frontClubMemberList = new List<FrontClubMember>();
            var isMember = await App.dbWrapper.IsMember(club.Id);
            var founderAccount = await App.dbWrapper.GetAccount(club.founderId);
            var prevRating = await App.dbWrapper.GetUserRating(club.Id);
            for (int i = 0; i < usersList.Count; i++)
            {
                frontClubMemberList.Add(new FrontClubMember(usersList[i], await App.dbWrapper.GetFriendship(usersList[i].Id)));

            }
            await Navigation.PushAsync(new ChatInfoPage(tagsList, club, frontClubMemberList, isMember, founderAccount.Username, prevRating));
        }
    }
}
