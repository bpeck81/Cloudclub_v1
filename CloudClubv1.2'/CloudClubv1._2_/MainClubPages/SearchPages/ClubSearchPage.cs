using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using CloudClubv1._2_;
using Backend;

namespace FrontEnd
{
    public class ClubSearchPage : ContentPage
    {

        ListView listView;
        ObservableCollection<FrontClub> frontClubList;
        public CreateClubPage createClubPage;
        public string title = "Explore";
        ColorHandler ch;
        string currentPage;
        List<Club> clubList, clubMemberList, popularClubs, newestClubs, returnedSearchedClubs;
        List<string> pendingInviteList;
        bool isBusy;
        public ClubSearchPage(List<Club> clubList, List<Club> clubMemberList, List<Club> popularClubs, List<Club> newestClubs, List<string> pendingInviteList)
        {
            ch = new ColorHandler();

            this.pendingInviteList = pendingInviteList;
            this.clubList = clubList;
            this.clubMemberList = clubMemberList;
            this.popularClubs = popularClubs;
            this.newestClubs = newestClubs;
            BackgroundColor = ch.fromStringToColor("purple");
            this.Icon = "ClubSearch_TabView.png";
            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);
            returnedSearchedClubs = new List<Club>();
            MessagingCenter.Subscribe<ClubSearchViewCell, string>(this, "Hi", async (sender, arg) => {

                var clubId = (string)arg;
                var answer = await DisplayAlert("Report", "Do you really want to report this club?", "Yes", "No");
                if (answer)
                {
                    await App.dbWrapper.CreateClubReport(clubId, App.dbWrapper.GetUser().Id);
                }


                //DisplayAlert("Report", "This club has been reported.","Ok");
            });




            Content = generatePopularPage();

        }

        private View generatePopularPage()
        {
            currentPage = "Popular";
            modClubList(popularClubs, clubMemberList);


            listView = new ListView
            {
                ItemsSource = frontClubList,
                ItemTemplate = new DataTemplate(typeof(ClubSearchViewCell)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowHeight = 160,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ch.fromStringToColor("white"),
                IsPullToRefreshEnabled = true,
                SeparatorColor = ch.fromStringToColor("lightGray")

            };
            listView.ItemSelected += ListView_ItemSelected;
            listView.RefreshCommand = new Command(() =>
            {
                updateData();
                modClubList(popularClubs, clubMemberList);
                listView.EndRefresh();

            });

            // listView.Refreshing += popularListViewRefresh;

            Button bCreateClub = new Button
            {
                Text = "+",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                BorderRadius = 0,
                BackgroundColor = ch.fromStringToColor("purple"),
                //   HeightRequest = 60,
                WidthRequest = 60,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };
            bCreateClub.Clicked += BCreateClub_Clicked;

            Button bPopularPage = new Button
            {
                Text = "Popular",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("purplePressed"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderRadius = 0,
                VerticalOptions = LayoutOptions.Center
            };
            bPopularPage.Clicked += BPopularPage_Clicked;
            Button bNewClubPage = new Button
            {
                Text = "New",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("purple"),
                BorderRadius = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bNewClubPage.Clicked += BNewClubPage_Clicked;
            Button bSearchClubsPage = new Button
            {
                Text = "Search",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("purple"),
                BorderRadius = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bSearchClubsPage.Clicked += BSearchClubsPage_Clicked;

            StackLayout bottomButtonLayout = new StackLayout
            {
                Children =
                {
                    bPopularPage,
                    bNewClubPage,
                    bSearchClubsPage,
                    bCreateClub
                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = ch.fromStringToColor("gray"),
                Spacing = 1
            };


            return new StackLayout
            {
                Children = {
                   listView,
                    bottomButtonLayout
                }
            };


        }


        private View generateNewestPage()
        {
            currentPage = "Newest";
            modClubList(newestClubs, clubMemberList);

            listView = new ListView
            {
                ItemsSource = frontClubList,
                ItemTemplate = new DataTemplate(typeof(ClubSearchViewCell)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowHeight = 160,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ch.fromStringToColor("white"),
                SeparatorColor = ch.fromStringToColor("lightGray")
            };

            listView.ItemSelected += ListView_ItemSelected;
            listView.RefreshCommand = new Command(() =>
            {
                updateData();
                modClubList(popularClubs, clubMemberList);
                listView.EndRefresh();

            });
            Button bCreateClub = new Button
            {
                Text = "+",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                BorderRadius = 0,
                BackgroundColor = ch.fromStringToColor("purple"),
                //   HeightRequest = 60,
                WidthRequest = 60,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };
            bCreateClub.Clicked += BCreateClub_Clicked;

            Button bPopularPage = new Button
            {
                Text = "Popular",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("purple"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderRadius = 0,
                VerticalOptions = LayoutOptions.Center
            };
            bPopularPage.Clicked += BPopularPage_Clicked;
            Button bNewClubPage = new Button
            {
                Text = "New",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("purplePressed"),
                BorderRadius = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bNewClubPage.Clicked += BNewClubPage_Clicked;
            Button bSearchClubsPage = new Button
            {
                Text = "Search",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("purple"),
                BorderRadius = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bSearchClubsPage.Clicked += BSearchClubsPage_Clicked; ;

            StackLayout bottomButtonLayout = new StackLayout
            {
                Children =
                {
                    bPopularPage,
                    bNewClubPage,
                    bSearchClubsPage,
                    bCreateClub
                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = ch.fromStringToColor("gray"),
                Spacing = 1
            };


            return new StackLayout
            {
                Children = {
                    listView,
                    bottomButtonLayout
                }
            };

        }
        private View generateSearchPage()
        {
            currentPage = "search";
            Entry searchEntry = new Entry
            {
                TextColor = ch.fromStringToColor("black"),
                BackgroundColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start
            };



            modClubList(returnedSearchedClubs, clubMemberList);
            ListView listView = new ListView
            {
                ItemsSource = frontClubList,
                ItemTemplate = new DataTemplate(typeof(ClubSearchViewCell)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowHeight = 160,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = ch.fromStringToColor("white"),
                SeparatorColor = ch.fromStringToColor("gray")
            };
            listView.ItemSelected += ListView_ItemSelected;
            Button bCreateClub = new Button
            {
                Text = "+",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                BorderRadius = 0,
                BackgroundColor = ch.fromStringToColor("purple"),
                //   HeightRequest = 60,
                WidthRequest = 60,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };
            bCreateClub.Clicked += BCreateClub_Clicked;

            Button bPopularPage = new Button
            {
                Text = "Popular",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("purple"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderRadius = 0,
                VerticalOptions = LayoutOptions.Center
            };
            bPopularPage.Clicked += BPopularPage_Clicked;
            Button bNewClubPage = new Button
            {
                Text = "New",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("purple"),
                BorderRadius = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bNewClubPage.Clicked += BNewClubPage_Clicked;
            Button bSearchClubsPage = new Button
            {
                Text = "Search",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("purplePressed"),
                BorderRadius = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            bSearchClubsPage.Clicked += BSearchClubsPage_Clicked; ;

            StackLayout bottomButtonLayout = new StackLayout
            {
                Children =
                {
                    bPopularPage,
                    bNewClubPage,
                    bSearchClubsPage,
                    bCreateClub
                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = ch.fromStringToColor("gray"),
                Spacing = 1
            };
            searchEntry.Focused += (sender, e) =>
            {
                bottomButtonLayout.IsVisible = false;
            };
            searchEntry.Unfocused += (sender, e) =>
            {
                bottomButtonLayout.IsVisible = true;
            };
            searchEntry.Completed += async (sender, e) =>
            {
                //TODO see how to get tags to search by
                List<string> tagsList = new List<string>();
                string tagString = searchEntry.Text;
                var tagsArray = tagString.Split(' ');
                tagsList = tagsArray.ToList();
                if (tagsList.Count != 0) returnedSearchedClubs = await App.dbWrapper.SearchClubs(tagsList);


                bottomButtonLayout.IsVisible = true;
                searchEntry.Text = "";
                this.Content = generateSearchPage();
            };
            return new StackLayout
            {
                Children = {
                    searchEntry,
                    new ScrollView
                        {
                            Content = listView,
                            Orientation = ScrollOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            BackgroundColor = Color.White
                        },
                    bottomButtonLayout,

                },
                BackgroundColor = ch.fromStringToColor("lightGray")

            };
        }

        private void BSearchClubsPage_Clicked(object sender, EventArgs e)
        {
            //check other list joined properties
            updateData();
            this.Content = this.generateSearchPage();
        }

        private void BNewClubPage_Clicked(object sender, EventArgs e)
        {
            updateData();
            this.Content = this.generateNewestPage();
        }

        private void BPopularPage_Clicked(object sender, EventArgs e)
        {
            updateData();
            this.Content = this.generatePopularPage();
        }

        private void modClubList(List<Club> clubList, List<Club> memberClubList)
        {
            frontClubList = new ObservableCollection<FrontClub>();

            for (int i = 0; i < clubList.Count; i++)
            {
                bool isMember = false;
                bool pendingInvite = false;

                for (int j = 0; j < memberClubList.Count; j++)
                {
                    if (clubList[i].Id.Equals(memberClubList[j].Id))
                    {
                        isMember = true;
                    }

                }
                for (int j = 0; j < pendingInviteList.Count; j++)
                {
                    if (pendingInviteList[j] == clubList[i].Id) pendingInvite = true;
                }

                FrontClub fClub = new FrontClub(clubList[i], isMember, pendingInvite);
                frontClubList.Add(fClub);

            }
        }

        private void BCreateClub_Clicked(object sender, EventArgs e)
        {
            createClubPage = new CreateClubPage();
            Navigation.PushAsync(createClubPage);
        }
        public async void updateData()
        {
            popularClubs = await App.dbWrapper.GetPopularClubs();
            newestClubs = await App.dbWrapper.GetNewestClubs();
            clubMemberList = await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id);
            clubList = await App.dbWrapper.GetClubs();

            pendingInviteList = new List<string>();

            for (int i = 0; i < clubList.Count; i++)
            {
                if (await App.dbWrapper.IsPendingClubRequest(clubList[i].Id))
                {
                    pendingInviteList.Add(clubList[i].Id);
                }
            }

        }
        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var club = (FrontClub)e.SelectedItem;
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

            bool isMember = await App.dbWrapper.IsMember(club.Id);
            await Navigation.PushAsync(new ClubChatPage(club, chatList, commentUsersList, requestUsersList, isMember));
            updateData();

            switch (currentPage)
            {
                case "Popular":
                    this.Content = generatePopularPage();
                    break;
                case "Newest":
                    this.Content = generateNewestPage();
                    break;
                case "Search":
                    this.Content = generateSearchPage();
                    break;
                default:
                    this.Content = generatePopularPage();
                    break;
            }



        }

        private void popularListViewRefresh(object sender, EventArgs e)
        {

            updateData();
            listView.EndRefresh();
            this.Content = generatePopularPage();

        }





    }

}

