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
        List<Club> clubList, clubMemberList, popularClubs, newestClubs;
        View popularPageContent, newestPageContent, searchPageContent;

        public ClubSearchPage(List<Club> clubList, List<Club> clubMemberList, List<Club> popularClubs, List<Club> newestClubs)
        {
            this.clubList = clubList;
            this.clubMemberList = clubMemberList;
            this.popularClubs = popularClubs;
            this.newestClubs = newestClubs;
            this.Icon = "ClubSearch_TabView.png";
            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);

           popularPageContent = generatePopularPage();
            newestPageContent = generateNewestPage();
            searchPageContent = generateSearchPage();
            Content = popularPageContent;

        }

        private View generatePopularPage()
        {

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
            Entry searchEntry = new Entry
            {
                TextColor = ch.fromStringToColor("black"),
                BackgroundColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start
            };


            List<Club> returnedSearchedClubs = new List<Club>();


            ListView listView = new ListView
            {
                ItemsSource = returnedSearchedClubs,
                ItemTemplate = new DataTemplate(typeof(ClubSearchViewCell)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowHeight = 150,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = ch.fromStringToColor("lightGray")
            };
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
                
                returnedSearchedClubs = await App.dbWrapper.SearchClubs(tagsList);
                
                bottomButtonLayout.IsVisible = true;
                searchEntry.Text = "";
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
            this.Content = this.searchPageContent;
        }

        private void BNewClubPage_Clicked(object sender, EventArgs e)
        {
            this.Content = this.newestPageContent;
        }

        private void BPopularPage_Clicked(object sender, EventArgs e)
        {
            this.Content = this.popularPageContent;
        }

        private List<FrontClub> modClubList(List<Club> clubList, List<Club> memberClubList)
        {
            List<FrontClub> frontClubList = new List<FrontClub>();
            for (int i = 0; i < clubList.Count; i++)
            {
                bool member = false;
                if (memberClubList.Contains(clubList[i]))
                {
                    member = true;
                }
                FrontClub fClub = new FrontClub(clubList[i], member);
                frontClubList.Add(fClub);

            }
            return frontClubList;
        }

        private void BCreateClub_Clicked(object sender, EventArgs e)
        {
            createClubPage = new CreateClubPage();
            Navigation.PushAsync(createClubPage);
        }
        
        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var club =(FrontClub) e.SelectedItem;
            var commentsList = await App.dbWrapper.GetComments(club.Id);
            List<Account> clubUsersList = new List<Account>();
            for(int i  = 0; i<commentsList.Count; i++)
            {
                clubUsersList.Add(await App.dbWrapper.GetAccount(commentsList[i].AuthorId));
            }
            /*
            //gets all users in comments page
            for(int i  = 0; i <commentsList.Count; i++)
            {
                bool inCommentsList = false;
                var commentAuth = await App.dbWrapper.GetAccount(commentsList[i].AuthorId);
                for(int j = 0; j< clubUsersList.Count; j++)
                {
                    if(clubUsersList[j] == commentAuth)
                    {
                        inCommentsList = true;
                    }
                    else{
                        inCommentsList = false;
                    }
                }
                if(inCommentsList == false)
                {
                    clubUsersList.Add(commentAuth);
                }

                
            }
            */
            //send in userlist that has same number of elements and corresponds directly with commentslist
            await Navigation.PushAsync(new ClubChatPage(club,commentsList, clubUsersList));



        }

    }
}
