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
    public class SettingsNotificationspage : ContentPage
    {
        ColorHandler ch;
        public SettingsNotificationspage()
        {
            ch = new ColorHandler();
            Title = "Notifications";

            var dailyRankSwitch = new SwitchCell
            {
                Text = "Daily Rank",
                On  = false                

            };
            dailyRankSwitch.OnChanged += async (sender, e) =>
            {
                if (dailyRankSwitch.On)
                {
                    await App.dbWrapper.EnableRankingNotification();
                

                }
                else
                {
                    await App.dbWrapper.DisableRankingNotification();
                }
            };
            var usersNearYouSwitch = new SwitchCell
            {
                Text = "Users Near You"                
                
            };
            usersNearYouSwitch.OnChanged += async (sender, e) =>
             {
                 throw new NotImplementedException();
             };

            Content = new StackLayout
            {
                Children =
                {
                    new TableView
                    {
                        BackgroundColor = ch.fromStringToColor("white"),
                        Root = new TableRoot
                        {
                            new TableSection
                            {
                                dailyRankSwitch,
                                usersNearYouSwitch
                            }
                        }
                    }
                }
            };
        }
    }
}
