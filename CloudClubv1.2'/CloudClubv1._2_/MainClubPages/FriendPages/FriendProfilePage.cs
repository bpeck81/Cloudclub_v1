using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Backend;
using Xamarin.Forms;
using CloudClubv1._2_;

namespace FrontEnd
{
    public class FriendProfilePage : ContentPage
    {
        Account user;
        ColorHandler ch;
        StackLayout bottomLayout;
        Label clubsLabel, lFriendRequestReceived, lAccountName, lMedals, lDroplet, userText;
        Image userEmoji, medalsImg, dropletImg;
        Button bNumClubs, bAccept, bReject, bSendFriendRequest;
        FriendRequest friendRequest;
        int activeFriendRequest;
        public FriendProfilePage(Account user, int activeFriendRequest, FriendRequest friendRequest = null)
        {
            //activeFriendRequest key 0 = not friends 1 = pending request from them 2 =friends 3= pending request from you

            ch = new ColorHandler();
            this.activeFriendRequest = activeFriendRequest;
            this.friendRequest = friendRequest;
            this.user = user;
            BackgroundColor = ch.fromStringToColor("purple");
            this.ToolbarItems.Add(new ToolbarItem
            {
                Icon = "Settings_Top.png",

                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => menuPopup())
            });

            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);
            updateView();

        }
        private void updateView()
        {
            lAccountName = new Label
            {
                Text = user.Username,
                TextColor = ch.fromStringToColor(user.Color),
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            userEmoji = new Image
            {
                Source = ImageSource.FromFile("Dog_Character.png"),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 115,
                Scale = .8
            };
            medalsImg = new Image
            {
                Source = ImageSource.FromFile("Medal_WhiteB.png"),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 50
            };
            lMedals = new Label
            {
                Text = 5.ToString(),//TODO: Update medals
                TextColor = ch.fromStringToColor("yellow"),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

            };
            var dropletPath = "DropletFull_WhiteB.png";
            if (user.NumDroplets > 0) dropletPath = "DropletFull_WhiteB.png";
            dropletImg = new Image
            {
                Source = ImageSource.FromFile(dropletPath),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 50
            };
            lDroplet = new Label
            {
                Text = user.NumDroplets.ToString(),
                TextColor = ch.fromStringToColor("blue"),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };



            userText = new Label
            {
                Text = user.Description,
                HorizontalOptions = LayoutOptions.CenterAndExpand,

            };
            clubsLabel = new Label
            {
                Text = "Clubs",
                TextColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 30
            };
            bNumClubs = new Button
            {
                Text = user.NumClubsIn.ToString(),
                TextColor = ch.fromStringToColor("white"),
                BackgroundColor = ch.fromStringToColor("green"),
                HeightRequest = 70,
                WidthRequest = 120,
                BorderRadius = 15,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            bNumClubs.Clicked += BNumClubs_Clicked;
            lFriendRequestReceived = new Label
            {
                Text = "Friend Request Received",
                TextColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 30
            };
            bAccept = new Button
            {
                Text = "Accept",
                TextColor = ch.fromStringToColor("white"),
                BackgroundColor = ch.fromStringToColor("green"),
                HeightRequest = 75,
                FontAttributes = FontAttributes.Bold,
                BorderRadius = 15,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            bAccept.Clicked += BAccept_Clicked;
            bReject = new Button
            {
                Text = "Reject",
                TextColor = ch.fromStringToColor("white"),
                BackgroundColor = ch.fromStringToColor("red"),
                HeightRequest = 75,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BorderRadius = 15
            };
            bReject.Clicked += BReject_Clicked;
            bSendFriendRequest = new Button
            {
                Text = "Send Request",
                BackgroundColor = ch.fromStringToColor("green"),
                TextColor = ch.fromStringToColor("white"),

                WidthRequest = 120,
                HeightRequest = 70,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BorderRadius = 15
            };
            bSendFriendRequest.Clicked += BSendFriendRequest_Clicked;

            var medalSLayout = new StackLayout
            {
                Children =
                {
                    medalsImg,
                    lMedals
                                },
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            var dropletSLayout = new StackLayout
            {
                Children =
                {
                    dropletImg,
                    lDroplet

                },
                HorizontalOptions = LayoutOptions.FillAndExpand

            };

            var medalsEmojiDropletLayout = new StackLayout
            {
                Children =
                {
                    medalSLayout,
                    userEmoji,
                    dropletSLayout
                },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(20, 40, 20, 20),
                Orientation = StackOrientation.Horizontal
            };
            var topLayout = new StackLayout
            {
                Children =
                {
                    medalsEmojiDropletLayout,
                    lAccountName,
                    userText
                },
                Spacing = 15,
                BackgroundColor = ch.fromStringToColor("white"),
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            bottomLayout = getBottomlayout();

            Content = new StackLayout
            {
                Children =
                {
                    topLayout,
                    bottomLayout
                }
            };
        }

        private async void BSendFriendRequest_Clicked(object sender, EventArgs e)
        {
            await App.dbWrapper.CreateFriendRequest(user.Id);
        }

        private StackLayout getBottomlayout()
        {
            if (activeFriendRequest == 2)
            {
                return new StackLayout
                {
                    Children =
                    {
                        clubsLabel,
                        bNumClubs
                    },
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(15, 10, 15, 15)

                };

            }
            else if (activeFriendRequest == 1)
            {
                return new StackLayout
                {
                    Children =
                    {
                        lFriendRequestReceived,
                        bAccept,
                        bReject
                    },
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(15, 10, 15, 15)

                };
            }

            else if (activeFriendRequest == 0)
            {
                return new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = "Request To Be friends",
                            TextColor = ch.fromStringToColor("white"),
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 30,
                            HorizontalOptions = LayoutOptions.CenterAndExpand
                        },
                        bSendFriendRequest
                    },
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Padding = new Thickness(15, 10, 15, 15)
                };
            }
            else
            {
                return new StackLayout
                {
                    Children =
                    {
                        new Label
                        {
                           Text = "Your Friend Request Is Pending",
                           TextColor = ch.fromStringToColor("black"),
                           FontAttributes = FontAttributes.Bold,
                           FontSize = 30,
                           HorizontalOptions = LayoutOptions.CenterAndExpand,
                           VerticalOptions = LayoutOptions.CenterAndExpand

                        }
                    },
                    BackgroundColor = ch.fromStringToColor("white"),
                    VerticalOptions = LayoutOptions.FillAndExpand,

                    Padding = new Thickness(15, 10, 15, 15)

                };
            }

        }
        private async void BNumClubs_Clicked(object sender, EventArgs e)
        {
            //TODO: change method for getting membership list
            var memberList = new List<Club>();
            var allClubs = await App.dbWrapper.GetClubs();
            for (int i = 0; i < allClubs.Count; i++)
            {
                if (await App.dbWrapper.IsClubMember(allClubs[i].Id, user.Id))
                {
                    memberList.Add(allClubs[i]);
                }
            }
            await Navigation.PushAsync(new FriendsClubListPage(memberList));
        }

        private async void BReject_Clicked(object sender, EventArgs e)
        {
            await App.dbWrapper.DeclineFriendRequest(friendRequest.Id);
            activeFriendRequest = await App.dbWrapper.GetFriendship(user.Id);
            updateView();
            await Navigation.PopAsync();
        }

        private async void BAccept_Clicked(object sender, EventArgs e)
        {
            await App.dbWrapper.AcceptFriendRequest(friendRequest.Id);
            activeFriendRequest = await App.dbWrapper.GetFriendship(user.Id);
            updateView();
            await Navigation.PopAsync();
        }

        private void menuPopup()
        {
            Navigation.PushAsync(new FriendSettingsPage());
        }

    }
}
