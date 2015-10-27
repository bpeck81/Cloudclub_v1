using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using CloudClubv1._2_;
using Backend;
using Xamarin.Forms;

namespace FrontEnd
{
    public class ClubChatPage : ContentPage
    {
        List<FrontComment> commentsList;
        ColorHandler ch;
        Entry userEntry;
        FrontClub club;
        public ClubChatPage(FrontClub club, List<Comment> commentsList, List<Account> users)
        {
            this.club = club;
            ch = new ColorHandler();
            this.Title = club.Title;
            this.commentsList = new List<FrontComment>();
            for (int i = 0; i < commentsList.Count;i++)
            {
                if(users[i] != null)
                {
                    this.commentsList.Add(new FrontComment(commentsList[i], users[i]));

                }
            }

            ListView listView = new ListView
            {
                ItemsSource = this.commentsList,
                ItemTemplate = new DataTemplate(typeof(CommentViewCell))
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
              VerticalOptions=  LayoutOptions.FillAndExpand
            };
                        
        }

        private async void UserEntry_Completed(object sender, EventArgs e)
        {
            if(userEntry.Text != "")
            {
                await App.dbWrapper.JoinClub(club.Id);
                await App.dbWrapper.CreateComment(userEntry.Text, club.Id);
                userEntry.Text = "";
            }


        }
    }
}
