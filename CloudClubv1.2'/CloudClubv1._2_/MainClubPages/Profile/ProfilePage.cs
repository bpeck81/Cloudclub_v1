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
    public class ProfilePage : ContentPage
    {
        ColorHandler ch;
        // Account account;
        public string title = "Profile";
        public List<FriendRequest> friendRequests;
        public List<Medal> medals;
        TapGestureRecognizer friendsImagetgr;
        TapGestureRecognizer newsImagetgr;
        TapGestureRecognizer userTextTgr;
        Label userText;
        Entry userTextEntry;

        public ProfilePage()
        {
            friendsImagetgr = new TapGestureRecognizer();
            friendsImagetgr.Tapped += FriendsImagetgr_Tapped;
            newsImagetgr = new TapGestureRecognizer();
            newsImagetgr.Tapped += NewsImagetgr_Tapped;
            userTextTgr = new TapGestureRecognizer();
            userTextTgr.Tapped += UserTextTgr_Tapped;
            ch = new ColorHandler();
            friendRequests = new List<FriendRequest>();
            medals = new List<Medal>();
            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);
            BackgroundColor = ch.fromStringToColor("white");
            Account user = App.dbWrapper.GetUser();

            Label lAccountName = new Label
            {
                Text = user.Username,
                TextColor = ch.fromStringToColor(user.Color),
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            Image userEmoji = new Image
            {
                Source = ImageSource.FromFile(user.Emoji),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 115,
                Scale = .8
            };
            Image medalsImg = new Image
            {
                Source = ImageSource.FromFile("Medal_WhiteB.png"),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 50
            };
            Label lMedals = new Label
            {
                Text = medals.Count.ToString(),
                TextColor = ch.fromStringToColor("yellow"),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

            };
            var dropletPath = "DropletFull_WhiteB.png";
            if (user.NumDroplets > 0) dropletPath = "DropletFull_WhiteB.png";
            Image dropletImg = new Image
            {
                Source = ImageSource.FromFile(dropletPath),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 50
            };
            Label lDroplet = new Label
            {
                Text = user.NumDroplets.ToString(),
                TextColor = ch.fromStringToColor("blue"),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            Label lInCloudClub = new Label
            {
                Text = "In Cloudclub",
                TextColor = ch.fromStringToColor("gray"),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            Label lNumClubs = new Label
            {
                Text = user.NumClubsIn.ToString(),
                FontSize = 42,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = ch.fromStringToColor("yellow")
            };
            userText = new Label
            {
                Text = user.Description,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))

            };
            userText.GestureRecognizers.Add(userTextTgr);
            userTextEntry = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = userText.Text,
                BackgroundColor = ch.fromStringToColor("white"),
                TextColor = ch.fromStringToColor("black"),
                IsVisible = false,
            };
            userTextEntry.TextChanged += UserTextEntry_TextChanged;
            userTextEntry.Completed += UserTextEntry_Completed;
            userTextEntry.Unfocused += (sender, e) => {
                userText.Text = userTextEntry.Text;
                userTextEntry.IsVisible = false;
                userText.IsVisible = true;
            };
            Image friendsImg = new Image
            {
                Source = FileImageSource.FromFile("Friends_Profile1.png"),
                Aspect = Aspect.AspectFit,
                WidthRequest = 187,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,

            };
            friendsImg.GestureRecognizers.Add(friendsImagetgr);

            Image newsImg = new Image
            {
                Source = FileImageSource.FromFile("News_Profile1.png"),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                WidthRequest = 187,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            newsImg.GestureRecognizers.Add(newsImagetgr);
            var medalSLayout = new StackLayout
            {
                Children =
                {
                    medalsImg,
                    lMedals
                },
                Orientation = StackOrientation.Horizontal

            };
            var dropletSLayout = new StackLayout
            {
                Children =
                {
                    dropletImg,
                    lDroplet
                },
                Orientation = StackOrientation.Horizontal
            };
            var medalsDropletStackLayout = new StackLayout
            {
                Children =
                {
                    medalSLayout,
                    dropletSLayout
                }

            };
            var inClubSLayout = new StackLayout
            {
                Children =
                {
                    lNumClubs,
                    lInCloudClub

                },
                Padding = new Thickness(0, 30, 0, 0)

            };
            var awardsEmojiInclubSLayout = new StackLayout
            {
                Children =
                {
                    medalsDropletStackLayout,
                    userEmoji,
                    inClubSLayout
                },
                Spacing = 35,
                Orientation = StackOrientation.Horizontal


            };
            var topLayout = new StackLayout
            {
                Children =
                {
                    awardsEmojiInclubSLayout,
                    lAccountName,
                    userText,
                    userTextEntry
                },
                Padding = new Thickness(20, 20, 20, 0),
                Spacing = 25
            };
            var friendsNewsSLayout = new StackLayout
            {
                Children =
                {
                    friendsImg,
                    newsImg
                },
                Spacing = 10,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var contentLayout = new StackLayout
            {
                Children =
                {
                    topLayout,
                    friendsNewsSLayout
                },
                Spacing = 10
            };

            Content = contentLayout;
        }

        private void UserTextEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            var val = entry.Text;
            if (entry.Text.Length > 50)
            {
                entry.Text = val.Remove(entry.Text.Length - 1);
            }
        }

        private void UserTextEntry_Completed(object sender, EventArgs e)
        {
            userText.Text = userTextEntry.Text;
            userTextEntry.IsVisible = false;
            userText.IsVisible = true;
        }

        private void UserTextTgr_Tapped(object sender, EventArgs e)
        {
            userText.IsVisible = false;
            userTextEntry.IsVisible = true;
            userTextEntry.Text = userText.Text;
        }

        private async void FriendsImagetgr_Tapped(object sender, EventArgs e)
        {
            var friendsList = await App.dbWrapper.GetFriends(App.dbWrapper.GetUser().Id);
            List<FrontFriends> frontFriendsList = new List<FrontFriends>();
            for (int i = 0; i < friendsList.Count; i++)
            {
                frontFriendsList.Add(new FrontFriends(friendsList[i], await App.dbWrapper.InSameClub(friendsList[i].Id)));
            }
            await Navigation.PushAsync(new FriendsPage(frontFriendsList));
        }

        private async void NewsImagetgr_Tapped(object sender, EventArgs e)
        {
            var news = await App.dbWrapper.GetNewsFeed();
            string friendRequest = "A friend";
            for (int i = 0; i < news.Count; i++)
            {
                if (news[i].GetType() == typeof(FriendRequest))
                {
                    var item = (FriendRequest)news[i];

                    var friend = await App.dbWrapper.GetAccount(item.AuthorId);
                    friendRequest = friend.Username;
                }
                else if (news[i].GetType() == typeof(Invite))
                //TODO add invite get club
                {
                    var item = (Invite)news[i];
                    var inviteAuthor = await App.dbWrapper.GetAccount(item.AuthorId);
                }
            }

            await Navigation.PushAsync(new NewsPage(news, friendRequest));
        }



        private string getUserEmojiString()
        {
            string defaultImageString = "cloud.png";

            return defaultImageString;


        }
        private string getUserDropletString()
        {
            string dropletString = "";

            return dropletString;
        }
    }
}
