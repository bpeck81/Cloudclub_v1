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



        public ProfilePage()
        {
            friendsImagetgr = new TapGestureRecognizer();
            friendsImagetgr.Tapped += FriendsImagetgr_Tapped;
            newsImagetgr = new TapGestureRecognizer();
            newsImagetgr.Tapped += NewsImagetgr_Tapped;
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
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            Image userEmoji = new Image
            {
                Source = ImageSource.FromFile(user.Emoji),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = .3
            };
            Image medalsImg = new Image
            {
                Source = ImageSource.FromFile("Medal_WhiteB.png"),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = .6
            };
            Label lMedals = new Label
            {
                Text = medals.Count.ToString(),
                TextColor = ch.fromStringToColor("yellow"),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Scale = .6
            };
            var dropletPath = "DropletFull_WhiteB.png";
            if (user.NumDroplets > 0) dropletPath = "DropletFull_WhiteB.png";
            Image dropletImg = new Image
            {
                Source = ImageSource.FromFile(dropletPath),
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
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
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            Label lNumClubs = new Label
            {
                Text = user.NumClubsIn.ToString(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            Image friendsImg = new Image
            {
                Source = FileImageSource.FromFile("Friends_Profile.png"),
                Aspect = Aspect.AspectFill,
                HorizontalOptions= LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand

            };
            friendsImg.GestureRecognizers.Add(friendsImagetgr);
            
            Image newsImg = new Image
            {
                Source = FileImageSource.FromFile("News_Profile.png"),
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            newsImg.GestureRecognizers.Add(newsImagetgr);

            StackLayout contentStackLayout = new StackLayout
            {
                Children =
                {
                    new StackLayout
                    {
                        Children =
                        {
                            new StackLayout
                            {
                               Children =
                                {
                                     new StackLayout
                                      {
                                        Children =
                                         {
                                             medalsImg,
                                             lMedals

                                         },
                                        Orientation = StackOrientation.Horizontal
                                       },
                                      new StackLayout
                                        {
                                          Children=
                                          {
                                              dropletImg,
                                              lDroplet
                                          },
                                          Orientation=StackOrientation.Horizontal

                                        },

                                }
                            },
                            userEmoji,
                            new StackLayout
                            {
                                Children =
                                {
                                    lNumClubs,
                                    lInCloudClub
                                }
                            }
                        },
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions= LayoutOptions.FillAndExpand,
                        VerticalOptions= LayoutOptions.FillAndExpand
                    },
                    lAccountName,
                    new StackLayout
                    {
                        Children =
                        {
                            friendsImg,
                            newsImg
                        },
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10
                    }
                    //add account jargon
                    // add two button images of friends and news for stack layout
                },
                BackgroundColor = ch.fromStringToColor("white")

            };
            Content = contentStackLayout;
        }

        private void FriendsImagetgr_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FriendsPage());
        }

        private void NewsImagetgr_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewsPage());
        }



        private string getUserEmojiString()
        {
            string defaultImageString = "cloud.png";
            /*  if (account.Emoji != "default")
              {
                  return account.Emoji;
              }
              else
              {
                  return defaultImageString;
              }
              */
            return defaultImageString;


        }
        private string getUserDropletString()
        {
            string dropletString = "";
            /*
            if (account.NumDroplets > 0)
            {
                dropletString = "DropletFull_WhiteB.png";
            }
            else
            {
                dropletString = "DropletEmpty_WhiteB.png";
            }*/
            return dropletString;
        }
    }
}
