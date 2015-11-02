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
        FrontClub club;
        public ClubChatPage(FrontClub club, List<Comment> commentsList, List<Account> users)
        {
            this.club = club;
            ch = new ColorHandler();
            this.BackgroundColor = Color.Black;
            this.Title = club.Title;
            this.commentsList = new ObservableCollection<FrontComment>();
            for (int i = 0; i < commentsList.Count;i++)
            {
                if(users[i] != null)
                {
                    this.commentsList.Add(new FrontComment(commentsList[i], users[i]));

                }
            }
            CurrentCommentsList = this.commentsList;

            updatePage();

            //Michael's debug stuff
            System.Diagnostics.Debug.WriteLine("mydebug---" + club.Id);
            App.dbWrapper.SetCurrentClubId(club.Id);
                        
        }
        private void updatePage()
        {
            ListView listView = new ListView
            {
                ItemsSource = CurrentCommentsList,
                ItemTemplate = new DataTemplate(typeof(CommentViewCell)),
                HasUnevenRows = true
            };

            userEntry = new Entry
            {
                BackgroundColor = ch.fromStringToColor("white"),
                TextColor = ch.fromStringToColor("black")
            };
            userEntry.Completed += UserEntry_Completed;

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

        private async void UserEntry_Completed(object sender, EventArgs e)
        {
            if(userEntry.Text != "")
            {
                var joinClub = await App.dbWrapper.JoinClub(club.Id);
                var commentOutput = await App.dbWrapper.CreateComment(userEntry.Text, club.Id);
                System.Diagnostics.Debug.WriteLine("OUTPUT: "+joinClub);
                userEntry.Text = "";
            }


        }
    }
}
