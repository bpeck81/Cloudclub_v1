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
        Image bBack, bSettings;
        ListView listView;
        TapGestureRecognizer settingsTgr, backButtonTgr;
        public ClubChatPage(ParentFrontClub club, List<DBItem> chatList, List<Account> commentUsers, List<Account> requestUsersList, bool isMember)
        {
            this.isMember = isMember;
            settingsTgr = new TapGestureRecognizer();
            settingsTgr.Tapped += SettingsTgr_Tapped;
            backButtonTgr = new TapGestureRecognizer();
            backButtonTgr.Tapped += BackButtonTgr_Tapped;
            this.club = club;
            ch = new ColorHandler();
            this.BackgroundColor = Color.Black;
            this.Title = club.Title;



            NavigationPage.SetHasNavigationBar(this, false);
            this.commentsList = new ObservableCollection<FrontComment>();
            int clubRequestCount = 0;
            System.Diagnostics.Debug.WriteLine(chatList.Count.ToString());
            chatList.Reverse();
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

            bBack = new Image
            {
                Source = FileImageSource.FromFile("arrow_back.png"),
                WidthRequest=30,
               // Scale = ,

                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 30,
                Aspect = Aspect.AspectFill
            };
            bBack.GestureRecognizers.Add(backButtonTgr);

            var actionBarLabel = new Label
            {
                Text = this.club.Title,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = ch.fromStringToColor("white"),
                FontSize = 22,
                FontAttributes = FontAttributes.Bold
            };
             bSettings = new Image
            {
                Source = ImageSource.FromFile("settings.png"),
                Scale = .8,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bSettings.GestureRecognizers.Add(settingsTgr);


            var actionBarLayout = new StackLayout
            {
                Children =
                {
                    bBack,
                    actionBarLabel,
                    bSettings
                },
                HeightRequest= 30,
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = ch.fromStringToColor(this.club.clubColor),
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(10,10,0,10)
            };

            listView = new ListView
            {
                ItemsSource = CurrentCommentsList,
                ItemTemplate = new DataTemplate(typeof(CommentViewCell)),
                HasUnevenRows = true,
                
            };
            listView.ScrollTo(CurrentCommentsList[CurrentCommentsList.Count() - 1], ScrollToPosition.End, false);
            listView.ItemTapped += ListView_ItemTapped;

            MessagingCenter.Subscribe<CommentViewCell, FrontComment>(this, "hi", async (sender, args) =>
            {
                var comment = (FrontComment)args;
                var answer = await DisplayAlert("Report User", "Do you really want to report " + comment.AuthorUsername + "?", "Yes", "No");
                if (answer)
                {
                    await App.dbWrapper.CreateBan(comment.AuthorId, comment.Id, App.dbWrapper.GetUser().Id);
                    comment.ShowReport = false;
                    comment.UpdateProperty("ShowReport");

                }
                else
                {
                    
                }

                //updatePage();

            });


            userEntry = new Entry
            {
                BackgroundColor = ch.fromStringToColor("white"),
                TextColor = ch.fromStringToColor("black"),
                VerticalOptions = LayoutOptions.End,
                IsEnabled = isMember,
                Placeholder = "Tap to chat"
            };
            userEntry.Completed += UserEntry_Completed;
            userEntry.Focused += UserEntry_Focused;
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
                   actionBarLayout,
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
                        actionBarLayout,
                        lEmptyChat,
                        userEntry
                    },
                    BackgroundColor = ch.fromStringToColor("lightGray"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
            }
        }

        private void UserEntry_Focused(object sender, FocusEventArgs e)
        {
            
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (FrontComment)e.Item;
            for(int i = 0; i < CurrentCommentsList.Count; i++)
            {
                if (CurrentCommentsList[i].ShowReport==true)
                {
                    CurrentCommentsList[i].ShowReport = false;
                    CurrentCommentsList[i].UpdateProperty("ShowReport");
                }

            }
            for (int i = 0; i < CurrentCommentsList.Count; i++)
            {
                if (CurrentCommentsList[i].Id == item.Id && CurrentCommentsList[i].ClubRequestBool == false)
                {
                    CurrentCommentsList[i].ShowReport = true;
                    CurrentCommentsList[i].UpdateProperty("ShowReport");
                }

            }

         //   updatePage();
            
        }
        

        private async void UserEntry_Completed(object sender, EventArgs e)
        {
            if (userEntry.Text != "")
            {
               // var joinClub = await App.dbWrapper.JoinClub(club.Id);
                var commentOutput = await App.dbWrapper.CreateComment(userEntry.Text, club.Id);
                //System.Diagnostics.Debug.WriteLine("OUTPUT: "+joinClub);
                userEntry.Text = "";
                listView.ScrollTo(CurrentCommentsList[CurrentCommentsList.Count() - 1], ScrollToPosition.End, false);

                //  updatePage();
            }


        }
        private async void BackButtonTgr_Tapped(object sender, EventArgs e)
        {

            await App.dbWrapper.RemoveCurrentClubId();
            var btn = sender as Image;
            btn.IsEnabled = false;
            await Navigation.PopAsync();
            btn.IsEnabled = true;
        }
        private async void SettingsTgr_Tapped(object sender, EventArgs e)
        {
            //var btn = sender as TapGestureRecognizer;
            //btn.Tapped -= SettingsTgr_Tapped;
         //   bSettings.GestureRecognizers.Remove(settingsTgr);
            var tagsList = await App.dbWrapper.GetTags(club.Id);
            var usersList = await App.dbWrapper.GetClubMembers(club.Id);
            var frontClubMemberList = new List<FrontClubMember>();
            var isMember = await App.dbWrapper.IsMember(club.Id);
            var founderAccount = await App.dbWrapper.GetAccount(club.founderId);
            var prevRating = await App.dbWrapper.GetUserRating(club.Id);
            var myFriendRequests = await App.dbWrapper.GetFriendRequests();
            for (int i = 0; i < usersList.Count; i++)
            {
                var storedFriendship = await App.dbWrapper.GetFriendship(usersList[i].Id);
                
                if(storedFriendship == 1) //Indicates request was sent from either user
                {
                    //  var accReq = App.dbWrapper.GetAccountFriendRequests(usersList[i].Id);
                    storedFriendship = 3;
                    var accReq = new List<FriendRequest>();
                    for (int j = 0; j < myFriendRequests.Count; j++)
                    {
                        if (myFriendRequests[j].AuthorId == usersList[i].Id)
                        {
                            storedFriendship = 1;//indicates request was sent by other acc
                        }

                     }


                }
                if (usersList[i].Id == App.dbWrapper.GetUser().Id) storedFriendship= 4;

                frontClubMemberList.Add(new FrontClubMember(usersList[i], storedFriendship));

                

            }
            var btn = sender as Image;
            btn.GestureRecognizers.Remove(settingsTgr);
            btn.InputTransparent = true;
            await Navigation.PushAsync(new ChatInfoPage(tagsList, club, frontClubMemberList, isMember, founderAccount.Username, prevRating));
            btn.GestureRecognizers.Add(settingsTgr);
            btn.InputTransparent = false;
        }

    }
}
