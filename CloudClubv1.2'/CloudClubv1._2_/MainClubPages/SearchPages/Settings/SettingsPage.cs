using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using CloudClubv1._2_;
using PCLStorage;
using Xamarin.Forms;
namespace FrontEnd
{
    class SettingsPage : ContentPage
    {
        ColorHandler ch;
        List<string> savedCloudList;

        public SettingsPage()
        {
            Title = "Settings";
            ch = new ColorHandler();

            var cloudsTCell = new TextCell
            {
                Text = "Clouds",
                TextColor = ch.fromStringToColor("black")
            };
            cloudsTCell.Tapped += async (sender, e) =>
            {
                var location = await App.dbWrapper.GetLocation();
                var cloudsList = await App.dbWrapper.GetAvailableClouds(location[0], location[1]);
                this.getSavedClouds();
                await Navigation.PushAsync(new CloudsPage(cloudsList, savedCloudList));

            };

            var notificationsTCell = new TextCell
            {
                Text = "Notifications",
                TextColor = ch.fromStringToColor("black")
            };
            notificationsTCell.Tapped += (sender, e) =>
            {
                Navigation.PushAsync(new SettingsNotificationspage());
            };

            var tutorialTCell = new TextCell
            {
                Text = "Tutorial",
                TextColor = ch.fromStringToColor("black")
            };
            tutorialTCell.Tapped += (sender, e) =>
            {

                Navigation.PushAsync(new CarouselTutorialPageRedo());
            };
            var contactUsTCell = new TextCell
            {
                Text = "Contact Us",
                TextColor = ch.fromStringToColor("black")
            };
            contactUsTCell.Tapped += (sender, e) =>
            {
                Navigation.PushAsync(new ContactUsPage());
            };

            var signOutTCell = new TextCell
            {

                Text = "Sign Out",
                TextColor = ch.fromStringToColor("black"),
            };
            signOutTCell.Tapped += async (sender, args) =>
            {

                var response = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
                if (response)
                {
                    var fileSystem = FileSystem.Current.LocalStorage;
                    var exists = await fileSystem.CheckExistsAsync("PhoneData.txt");
                    if (exists.Equals(ExistenceCheckResult.FileExists))
                    {
                        IFile file = await fileSystem.CreateFileAsync("PhoneData.txt", CreationCollisionOption.ReplaceExisting);
                        string baseString = "a\nUVA,\n";
                        await file.WriteAllTextAsync(baseString);
                        var navPage = new NavigationPage(new CarouselTutorialPage());
                        Application.Current.MainPage = navPage;
                    }
                    else
                    {
                        throw new System.IO.FileNotFoundException("PhoneData.txt");
                    }
                }


            };
            // Navigation.PushModalAsync(new SignUpPage());

            TableView tableView = new TableView
            {
                Intent = TableIntent.Settings,
                Root = new TableRoot
                {
                    new TableSection
                    {
                        cloudsTCell,
                        notificationsTCell,
                        tutorialTCell,
                        contactUsTCell,
                        signOutTCell
                    }
                },
                BackgroundColor = ch.fromStringToColor("white"),

            };
            Content = tableView;


        }

        private async void getSavedClouds()
        {
            var saveFileKey = new SaveFileDictionary();
            savedCloudList = new List<string>();
            var fileSystem = FileSystem.Current.LocalStorage;
            var exists = await fileSystem.CheckExistsAsync("PhoneData.txt");
            if (exists.Equals(ExistenceCheckResult.FileExists))
            {
                var file = await fileSystem.GetFileAsync("PhoneData.txt");
                var text = await file.ReadAllTextAsync();
                var fileLines = text.Split('\n');
                var cloudsList = fileLines[saveFileKey.dict["CLOUDREGION"]].Split(',');
                System.Diagnostics.Debug.WriteLine(cloudsList[0].ToString());
                savedCloudList = cloudsList.ToList<string>();
            }
            else throw new System.IO.IOException();
        }
        private void SwitchCell_Tapped(object sender, EventArgs e)
        {
            //  App.dbWrapper.
            //// add turn on notifications 
        }
    }
}
