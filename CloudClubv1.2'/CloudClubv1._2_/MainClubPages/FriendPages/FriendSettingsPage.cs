using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Backend;
using CloudClubv1._2_;
using Xamarin.Forms;

namespace FrontEnd
{
    public class FriendSettingsPage : ContentPage
    {
        ColorHandler ch;
        string userId;
        public FriendSettingsPage(string userId)
        {
            this.userId = userId;
            ch = new ColorHandler();
            Title = "User Settings";
            BackgroundColor = ch.fromStringToColor("white");
            TableView tableView = new TableView
            {
                Root = new TableRoot()
            };
            TableSection tSection = new TableSection();
            tableView.Root.Add(tSection);
            TextCell removeFriendCell = new TextCell
            {
                Text = "Remove Friend",
                TextColor = ch.fromStringToColor("black"),
                              

            };
            removeFriendCell.Tapped += RemoveFriendCell_Tapped;
            TextCell reportTextCell = new TextCell
            {
                Text = "Report",
                TextColor = ch.fromStringToColor("black"),


            };
            reportTextCell.Tapped += ReportTextCell_Tapped;
            tSection.Add(removeFriendCell);
            tSection.Add(reportTextCell);
            Content = tableView;
        }

        private async void ReportTextCell_Tapped(object sender, EventArgs e)
        {
            //TODO: tie to backend

            var answer = await DisplayAlert("Reported Friend", "Do you really want to report your friend?", "Yes", "No");
            if (answer == true)
            {
                // await App.dbWrapper.CreateBan()
                await App.dbWrapper.CreateBan(userId, "", App.dbWrapper.GetUser().Id);
                await Navigation.PopAsync();
            }


        }

        private async void RemoveFriendCell_Tapped(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Removed Friend", "Do you really want to remove your Friend", "Yes", "No");

            throw new NotImplementedException();
            if (answer == true)
            {
                await Navigation.PopAsync();
            }
            

            //TODO:tie to backend
        }
    }
}
