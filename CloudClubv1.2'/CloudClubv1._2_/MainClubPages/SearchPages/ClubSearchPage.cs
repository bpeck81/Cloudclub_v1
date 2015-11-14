using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using CloudClubv1._2_;
using Backend;

namespace FrontEnd
{
    public class ClubSearchPage : ContentPage
    {

        ListView listView;
        List<FrontClub> frontClubList;
        public CreateClubPage createClubPage;
        public string title = "Explore";
        ColorHandler ch;
        string currentPage;
        List<Club> clubList, clubMemberList, popularClubs, newestClubs, returnedSearchedClubs;
        List<string> pendingInviteList;

        public ClubSearchPage(List<Club> clubList, List<Club> clubMemberList, List<Club> popularClubs, List<Club> newestClubs, List<string>pendingInviteList)
        {
            this.pendingInviteList = pendingInviteList;
            this.clubList = clubList;
            this.clubMemberList = clubMemberList;
            this.popularClubs = popularClubs;
            this.newestClubs = newestClubs;
            this.Icon = "ClubSearch_TabView.png";
            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);
            returnedSearchedClubs = new List<Club>();



            Content = generatePopularPage();

        }

        private View generatePopularPage()
        {
            currentPage = "Popular";
            frontClubList = new List<FrontClub>();
            ch = new ColorHandler();
            frontClubList = modClubList(popularClubs, clubMemberList);

            listView = new ListView
            {
                ItemsSource = frontClubList,
                ItemTemplate = new DataTemplate(typeof(ClubSearchViewCell)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowHeight = 150,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = ch.fromStringToColor("gray")
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

            StackLayout clubSearchLayout = new StackLayout
            {
                Children =
                {
                    listView,
                   // bCreateClub,
                    bottomButtonLayout
                },
                BackgroundColor = ch.fromStringToColor("lightGray")
            };

            return new StackLayout
            {
                Children = {
                    new ScrollView
                        {
                            Content = listView,
                            Orientation = ScrollOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            BackgroundColor = Color.White
                        },
                    bottomButtonLayout
                }
            };


        }

        private View generateNewestPage()
        {
            currentPage = "Newest";
            frontClubList = new List<FrontClub>();
            ch = new ColorHandler();
            frontClubList = modClubList(newestClubs, clubMemberList);

            listView = new ListView
            {
                ItemsSource = frontClubList,
                ItemTemplate = new DataTemplate(typeof(ClubSearchViewCell)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowHeight = 150,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = ch.fromStringToColor("gray")
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
                    new ScrollView
                        {
                            Content = listView,
                            Orientation = ScrollOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            BackgroundColor = Color.White
                        },
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



            var moddedItemSource = modClubList(returnedSearchedClubs, clubMemberList);
            ListView listView = new ListView
            {
                ItemsSource = moddedItemSource,
                ItemTemplate = new DataTemplate(typeof(ClubSearchViewCell)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowHeight = 150,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = ch.fromStringToColor("lightGray")
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

        private async void BSearchClubsPage_Clicked(object sender, EventArgs e)
        {
            //check other list joined properties
            updateData();
            this.Content = this.generateSearchPage();
        }

        private async void BNewClubPage_Clicked(object sender, EventArgs e)
        {
            updateData();
            this.Content = this.generateNewestPage();
        }

        private async void BPopularPage_Clicked(object sender, EventArgs e)
        {
            updateData();
            this.Content = this.generatePopularPage();
        }

        private List<FrontClub> modClubList(List<Club> clubList, List<Club> memberClubList)
        {
            List<FrontClub> frontClubList = new List<FrontClub>();

            for (int i = 0; i < clubList.Count; i++)
            {
                bool isMember = false;
                bool pendingInvite = false;

                for (int j=0; j<memberClubList.Count; j++)
                {
                    if (clubList[i].Id.Equals(memberClubList[j].Id))
                    {
                        isMember = true;
                    }
                   
                }
                for (int j = 0; j<pendingInviteList.Count; j++)
                {
                    if (pendingInviteList[j] == clubList[i].Id) pendingInvite = true;
                }

                FrontClub fClub = new FrontClub(clubList[i], isMember,pendingInvite);
                frontClubList.Add(fClub);

            }
            return frontClubList;
        }

        private void BCreateClub_Clicked(object sender, EventArgs e)
        {
            createClubPage = new CreateClubPage();
            Navigation.PushAsync(createClubPage);
        }
        private async void updateData()
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

            var club = (FrontClub)e.SelectedItem;
            var chatList = await App.dbWrapper.GetChat(club.Id,"", "");

            List<Account> requestUsersList = new List<Account>();
            List<Account> commentUsersList = new List<Account>();

            for(int i = 0; i< chatList.Count; i++)
            {
                if (chatList[i].GetType() == typeof(Comment))
                {
                    var comment = (Comment)chatList[i];
                    commentUsersList.Add(await App.dbWrapper.GetAccount(comment.AuthorId));

                }
                else if( chatList[i].GetType() == typeof(ClubRequest))
                {
                    var request = (ClubRequest)chatList[i];
                    requestUsersList.Add(await App.dbWrapper.GetAccount(request.AccountId));
                }
            }

            bool isMember = await App.dbWrapper.IsMember(club.Id);
            await Navigation.PushAsync(new ClubChatPage(club,chatList, commentUsersList, requestUsersList,isMember));
        
         }
           
            

        
        }

    }

