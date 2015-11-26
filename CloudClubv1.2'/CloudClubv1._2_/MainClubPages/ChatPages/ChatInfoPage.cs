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
    public class ChatInfoPage : ContentPage
    {
        ColorHandler ch;
        string currentPage;
        List<Tag> tagsList;
        ParentFrontClub club;
        List<Image> starImages;
        List<FrontClubMember> usersList;
        Button bottomButton, bSubscribe, bUnsubscribe, bRating, bTags;
        bool isMember;
        string founderUsername;
        TapGestureRecognizer starTap;
        Entry addTagEntry;
        int previousRating;
        public ChatInfoPage(List<Tag> tagsList, ParentFrontClub club, List<FrontClubMember> usersList, bool isMember, string founderUsername, int previousRating)
        {
            this.previousRating = previousRating;
            starTap = new TapGestureRecognizer();
            starTap.Tapped += StarTap_Tapped;
            this.tagsList = tagsList;
            this.founderUsername = founderUsername;
            this.isMember = isMember;
            this.club = club;
            this.usersList = usersList;
            currentPage = "tags";
            ch = new ColorHandler();
            Title = "Info";
            bottomButton = new Button();
            BackgroundColor = ch.fromStringToColor("white");
            starImages = new List<Image>();
            for (int i = 0; i < 5; i++)
            {
                Image star = new Image
                {
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 60,
                    BackgroundColor = ch.fromStringToColor("white"),
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                if (i <= previousRating)
                {
                    star.Source = ImageSource.FromFile(ch.getStarColorString(club.clubColor));

                }
                else
                {
                    star.Source = ImageSource.FromFile(ch.getStarColorString("gray"));
                }
                if (isMember)
                {
                    star.GestureRecognizers.Add(starTap);
                }
                starImages.Add(star);
            }

            MessagingCenter.Subscribe<FriendProfilePage>(this, "Refresh User Data", (sender) =>
             {
                 updateData();

                 updatePage();
             });
            MessagingCenter.Subscribe<ClubMemberViewCell>(this, "Refresh User Data", (sender) =>
             {
                 updateData();
                 updatePage();
             });

            bSubscribe = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Text = "Join Club",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = ch.fromStringToColor("green"),
                BorderRadius = 15,
                HeightRequest = 40

            };
            bSubscribe.Clicked += BSubscribe_Clicked;
            bUnsubscribe = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Text = "Leave Club",
                FontAttributes = FontAttributes.Bold,
                TextColor = ch.fromStringToColor("white"),
                BackgroundColor = ch.fromStringToColor("gray"),
                BorderRadius = 15,
                HeightRequest = 40
            };
            bUnsubscribe.Clicked += BUnsubscribe_Clicked;
            if (isMember)
            {
                this.bottomButton = bUnsubscribe;
            }
            else bottomButton = bSubscribe;
            bTags = new Button
            {
                Text = "Tags",
                TextColor = ch.fromStringToColor("red"),
                BackgroundColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderRadius = 0,
                BorderColor = ch.fromStringToColor("gray")
            };
            bTags.Clicked += BTags_Clicked;
            bRating = new Button
            {
                Text = "Rating",
                TextColor = ch.fromStringToColor("gray"),
                BackgroundColor = ch.fromStringToColor("white"),
                BorderRadius = 0,
                BorderColor = ch.fromStringToColor("gray"),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            bRating.Clicked += BRating_Clicked;
           /* MessagingCenter.Subscribe<ClubMemberViewCell, FrontClubMember>(this, "friendsProfilePage", async (sender, args) =>
            {
                var acc = (FrontClubMember)args;
                var account = await App.dbWrapper.GetAccount(acc.Id);
                int friendship = await App.dbWrapper.GetFriendship(acc.Id);
                var friendRequests = await App.dbWrapper.GetFriendRequests();
                for (int i = 0; i < friendRequests.Count; i++)
                {
                    if (friendship == 1)
                    {
                        if (friendRequests[i].AuthorId != acc.Id)
                        {
                            friendship = 1;
                        }
                        else
                        {
                            friendship = 3;
                        }
                    }

                }
                

                var accountPage = new FriendProfilePage(account, friendship);
                await Navigation.PushAsync(accountPage);

            });*/

            updatePage();

        }

        public void updatePage()
        {
            StackLayout topBarSLayout = new StackLayout
            {
                Children =
                {
                    bTags,
                    bRating,
                },
                BackgroundColor = ch.fromStringToColor("lightGray"),
                Spacing = 2,
                Orientation = StackOrientation.Horizontal

            };

            var usersListView = new ListView
            {
                ItemsSource = usersList,
                ItemTemplate = new DataTemplate(typeof(ClubMemberViewCell)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ch.fromStringToColor("white"),
                SeparatorColor = ch.fromStringToColor("lightGray"),
                HeightRequest = 150,
                RowHeight = 50

            };
            usersListView.ItemTapped += UsersListView_ItemTapped;
            

            var middleBar = getMiddleBar();
            StackLayout middleBandSLayout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "Founder: ",
                        TextColor = ch.fromStringToColor("yellow"),
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        HorizontalOptions = LayoutOptions.Start
                    },
                    new Label
                    {
                        Text = this.founderUsername,
                        TextColor = ch.fromStringToColor("yellow"),
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        HorizontalOptions = LayoutOptions.Center

                    },
                    },
                Orientation = StackOrientation.Horizontal,

            };
            var lMember = new Label
            {
                Text = "Members",
                TextColor = ch.fromStringToColor("red"),
                FontAttributes = FontAttributes.Bold,
                XAlign = TextAlignment.Center
            };



            Content = new StackLayout
            {
                Children =
                {
                    topBarSLayout,
                    middleBar,
                    middleBandSLayout,
                    lMember,
                    usersListView,
                    bottomButton
                },
                Padding = new Thickness(15, 5, 15, 15),
                Spacing = 15,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand

            };

        }

        private async void UsersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var user = (FrontClubMember)e.Item;
            var acc  = await App.dbWrapper.GetAccount(user.Id);

            if(user.friendship != 4) await Navigation.PushAsync(new FriendProfilePage(acc, user.friendship));
        }

        private StackLayout getMiddleBar()
        {
            if (currentPage == "tags")
            {
                var tagsListView = new ListView
                {
                    ItemsSource = tagsList,
                    RowHeight = 30,
                    SeparatorColor = ch.fromStringToColor("lightGray"),
                    
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    ItemTemplate = new DataTemplate(() =>
                    {
                        var label = new Label
                        {
                            BackgroundColor = ch.fromStringToColor("white"),
                            TextColor = ch.fromStringToColor("black")
                        };
                        label.SetBinding(Label.TextProperty, "Key");
                        return new ViewCell
                        {
                            View = label
                        };
                    }
                    )

                };
                bool entryVisible = false;
                if (App.dbWrapper.GetUser().Username == founderUsername) entryVisible = true;
                addTagEntry = new Entry
                {
                    TextColor = ch.fromStringToColor("black"),
                    IsVisible = entryVisible,
                    Placeholder = "tap to add tag",
                    IsEnabled = entryVisible,
                    BackgroundColor = ch.fromStringToColor("lightGray"),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    
                };
                addTagEntry.Completed += async(sender, args) =>
                {
                    this.tagsList.Add(new Tag(addTagEntry.Text, club.Id, club.cloudId));
                    var stringTagsList = new List<string>();
                    for(int i =0; i<tagsList.Count; i++)
                    {
                        stringTagsList.Add(tagsList[i].Key);
                    }
                    await App.dbWrapper.AddTags(club.Id, club.cloudId, stringTagsList);
                    updatePage();

                };
                return new StackLayout
                {
                    Children =
                    {
                        tagsListView,
                        addTagEntry
                    },
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HeightRequest = 70,
                    Spacing = 1,
                    BackgroundColor = ch.fromStringToColor("white"),
                    VerticalOptions = LayoutOptions.Center
                };
            }
            else
            {
                var starSLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Spacing = 2,
                    HeightRequest = 70,
                    Orientation = StackOrientation.Horizontal,
                    BackgroundColor = ch.fromStringToColor("white")
                };

                for (int i = 0; i < starImages.Count; i++)
                {
                    starSLayout.Children.Add(starImages[i]);
                }

                return starSLayout;
            }

        }

        private void StarTap_Tapped(object sender, EventArgs e)
        {
            Image starImage = (Image)sender;
            if (isMember)
            {
                for (int i = 0; i < starImages.Count; i++)
                {
                    starImages[i].Source = FileImageSource.FromFile(ch.getStarColorString("gray"));
                }
                for (int i = 0; i < starImages.Count; i++)
                {
                    if (starImages[i].Id == starImage.Id)
                    {
                        for (int j = 0; j <= i; j++)
                        {
                            starImages[j].Source = ImageSource.FromFile(ch.getStarColorString(club.clubColor));
                            App.dbWrapper.RateClub(j, club.Id);
                        }
                        break;
                    }
                }
            }
           
        }

        private async void BUnsubscribe_Clicked(object sender, EventArgs e)
        {
            await App.dbWrapper.LeaveClub(this.club.Id);

            isMember = false;
            this.bottomButton = bSubscribe;
            updatePage();

        }

        private async void BSubscribe_Clicked(object sender, EventArgs e)
        {
            await App.dbWrapper.JoinClub(club.Id);
            isMember = true;
            bottomButton = bUnsubscribe;
            updatePage();
        }
        private void BTags_Clicked(object sender, EventArgs e)
        {
            bRating.TextColor = ch.fromStringToColor("gray");
            bTags.TextColor = ch.fromStringToColor("red");
            currentPage = "tags";
            updatePage();
        }


        private void BRating_Clicked(object sender, EventArgs e)
        {
            bRating.TextColor = ch.fromStringToColor("red");
            bTags.TextColor = ch.fromStringToColor("gray");
            currentPage = "rating";
            updatePage();
        }

        private async void updateData()
        {
            var updatedUsersList = await App.dbWrapper.GetClubMembers(club.Id);
            var frontClubMemberList = new List<FrontClubMember>();
            for (int i = 0; i < updatedUsersList.Count; i++)
            {
                var storedFriendship = await App.dbWrapper.GetFriendship(updatedUsersList[i].Id);
                if (storedFriendship == 1) //Indicates request was sent from either user
                {
                    //  var accReq = App.dbWrapper.GetAccountFriendRequests(usersList[i].Id);
                    storedFriendship = 3;
                    var accReq = new List<FriendRequest>();
                    for (int j = 0; j < accReq.Count; j++)
                    {
                        if (accReq[i].AuthorId == App.dbWrapper.GetUser().Id)
                        {
                            storedFriendship = 3;//indicates request was sent by user, not other acc
                        }
                        else
                        {
                            storedFriendship = 1;
                        }
                    }


                }
                frontClubMemberList.Add(new FrontClubMember(updatedUsersList[i], storedFriendship));

            }
            this.usersList = frontClubMemberList;
        }
    }


}
