using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontEnd;
using System.IO;
using Backend;
using PCLStorage;
using Xamarin.Forms;

namespace CloudClubv1._2_
{
    public class App : Application
    {
        public static DBWrapperInterface dbWrapper;
        ColorHandler ch;

        public App(DBWrapperInterface myDBWrapper)
        {

            dbWrapper = myDBWrapper;

        }


        protected override async void OnStart()
        {
            ch = new ColorHandler();
            Dictionary<string, int> saveFileKey = new Dictionary<string, int>();
            saveFileKey.Add("USERID", 0);
            System.Diagnostics.Debug.WriteLine(FileSystem.Current.LocalStorage.Path);
            var fileSystem = FileSystem.Current.LocalStorage;
            var fileExists = await fileSystem.CheckExistsAsync("PhoneData.txt");
       //     createFileSystem(fileSystem);
            if (fileExists.Equals(ExistenceCheckResult.FileExists))
            {
                IFile file = await fileSystem.GetFileAsync("PhoneData.txt");
                var data = await file.ReadAllTextAsync();
                var dataLines = data.Split('\n');
                var idLoc = dataLines[saveFileKey["USERID"]].Split(':');
                string id = idLoc[1];
                if (id[id.Length - 1] == ';') id = id.Substring(0, id.Length - 2);
                System.Diagnostics.Debug.WriteLine(id);
                if (!id.Equals("a"))
                {
                    var userAccount = await dbWrapper.GetAccount(id);
                    await App.dbWrapper.LoginAccount(userAccount.Username, userAccount.Password);

                    var clubs = await App.dbWrapper.GetClubs();
                    var popularClubs = await App.dbWrapper.GetPopularClubs();
                    var newestClubs = await App.dbWrapper.GetNewestClubs();
                    var memberClubsList = await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id);
                    var pendingClubList = new List<string>();
                    //TODO check if get clubs returns all clubs
                    for (int i = 0; i < clubs.Count; i++)
                    {
                        if (await App.dbWrapper.IsPendingClubRequest(clubs[i].Id))
                        {
                            pendingClubList.Add(clubs[i].Id);
                        }

                    }

                    var navPage = new NavigationPage(new TabbedMainClubPages(clubs, memberClubsList, popularClubs, newestClubs, pendingClubList));

                    navPage.BarBackgroundColor = ch.fromStringToColor("purple");
                    MainPage = navPage;
                }
                else
                {
                    createFileSystem(fileSystem);
                }


            }
            else
            {

                createFileSystem(fileSystem);
            }


            // Handle when your app starts

        }
        public async void createFileSystem(IFolder fileSystem)
        {
            IFile file = await fileSystem.CreateFileAsync("PhoneData.txt", CreationCollisionOption.ReplaceExisting);
            string baseString = "USERID:a\nCLOUDREGION:UVA\n";
            await file.WriteAllTextAsync(baseString);
            var navPage = new NavigationPage(new CarouselTutorialPage());
            MainPage = navPage;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
