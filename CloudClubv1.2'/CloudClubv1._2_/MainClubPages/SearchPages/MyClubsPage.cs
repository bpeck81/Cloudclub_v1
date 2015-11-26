using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Backend;
using CloudClubv1._2_;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace FrontEnd
{
    public class MyClubsPage : ContentPage
    {
        ObservableCollection<FrontMyClub> frontMyClubList;
        public string title = "Subscriptions";
        ColorHandler ch;
        ListView listView;
        ScrollView clubScroll;
        Button bNewClub;
        public MyClubsPage(List<Club> memberClubList, List<string> recentCommentsList)
        {

            ch = new ColorHandler();

            generateDisplayList(memberClubList, recentCommentsList);
            updatePage();

        }

        private void updatePage()
        {

            listView = new ListView
            {
                ItemsSource = frontMyClubList,
                ItemTemplate = new DataTemplate(typeof(MyClubViewCell)),
                SeparatorColor = ch.fromStringToColor("gray"),
                HasUnevenRows = true,
                IsPullToRefreshEnabled = true
            };
            listView.ItemSelected += ListView_ItemSelected;
            //listView.ItemTapped += ListView_ItemSelected;
            listView.Refreshing += ListView_Refreshing;
            
            clubScroll = new ScrollView
            {
               
                Content = listView,
                Orientation = ScrollOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                
                BackgroundColor = Color.White
            };
            if (frontMyClubList.Count > 0)
            {
                Content = clubScroll;
            }
            else
            {
                bNewClub = new Button
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
                bNewClub.Clicked += async (object sender, EventArgs e) =>
                {
                    var btn = (Button)sender;
                    btn.IsEnabled = false;
                    await Navigation.PushAsync(new CreateClubPage());
                    btn.IsEnabled = true;
                   
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
                            Text = "Touch the left tab to explore and find interesting clubs!",
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

        private void ListView_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {

            var lView = (ListView)sender;
            for(int i = frontMyClubList.Count-1; i>=0; i--)
            {
                frontMyClubList.RemoveAt(i);
            }
            var memberClubsList = await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id);
            var commentList = new List<Comment>();
            for(int i =0; i<memberClubsList.Count; i++)
            {
                var recentComment = await App.dbWrapper.GetRecentComment(memberClubsList[i].Id);
                if (recentComment.Text == "") recentComment.Text = "The chat is empty!";
                frontMyClubList.Add(new FrontMyClub(memberClubsList[i], recentComment.Text));
            }
            lView.IsRefreshing = false;
            

        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var club = (FrontMyClub)e.SelectedItem;
            var chatList = await App.dbWrapper.GetChat(club.Id, "","");

            List<Account> requestUsersList = new List<Account>();
            List<Account> commentUsersList = new List<Account>();

            for (int i = 0; i < chatList.Count; i++)
            {
                if (chatList[i].GetType() == typeof(Comment))
                {
                    var comment = (Comment)chatList[i];
                    commentUsersList.Add(await App.dbWrapper.GetAccount(comment.AuthorId));

                }
                else if (chatList[i].GetType() == typeof(ClubRequest))
                {
                    var request = (ClubRequest)chatList[i];
                    requestUsersList.Add(await App.dbWrapper.GetAccount(request.AccountId));
                }
            }
            var memberClubList = await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id); 
            var isMember = await App.dbWrapper.IsMember(club.Id);
            await App.dbWrapper.SetCurrentClubId(club.Id);

            var ccp = new ClubChatPage(club, chatList, commentUsersList, requestUsersList, isMember);
            NavigationPage.SetHasNavigationBar(ccp, false);

            await Navigation.PushAsync(ccp);
            generateDisplayList(await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id), await App.getMostRecentComment(memberClubList));
            updatePage();

        }
        public async void updateData()
        {
            var memberClubsList = await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id);

            generateDisplayList(memberClubsList, await App.getMostRecentComment(memberClubsList));
            updatePage();
        }


        private void generateDisplayList(List<Club> clubList, List<string> commentList)
        {
            frontMyClubList = new ObservableCollection<FrontMyClub>();

            for (int i = 0; i < clubList.Count; i++)
            {
                frontMyClubList.Add(new FrontMyClub(clubList[i], commentList[i]));
            }


        }

    }
}
