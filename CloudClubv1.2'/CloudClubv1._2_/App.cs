using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontEnd;
using System.IO;
using Backend;
using PCLStorage;
using Xamarin.Forms;
using System.Threading.Tasks;
using FrontEnd;

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

         //   App.dbWrapper.
           
            var saveFileKey = new SaveFileDictionary();

            System.Diagnostics.Debug.WriteLine(FileSystem.Current.LocalStorage.Path);
            var fileSystem = FileSystem.Current.LocalStorage;
            var fileExists = await fileSystem.CheckExistsAsync("PhoneData.txt");
          //  createCleanFileSystem(fileSystem);
            if (fileExists.Equals(ExistenceCheckResult.FileExists))
            {
                IFile file = await fileSystem.GetFileAsync("PhoneData.txt");
                var data = await file.ReadAllTextAsync();
                var dataLines = data.Split('\n');
                //      var idLoc = dataLines[saveFileKey.dict["USERID"]].Split(':');
                //  string id = idLoc[1];
                string id = dataLines[saveFileKey.dict["USERID"]];
             //   if (id[id.Length - 1] == ';') id = id.Substring(0, id.Length - 2);
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
                    var firstLineCommentList = await App.getMostRecentComment(memberClubsList);
                  //  await App.dbWrapper.cloud
                  
                    var navPage = new NavigationPage(new TabbedMainClubPages(clubs, memberClubsList, popularClubs, newestClubs, pendingClubList, firstLineCommentList));

                    navPage.BarBackgroundColor = ch.fromStringToColor("purple");
                    MainPage = navPage;
                }
                else
                {
                    createCleanFileSystem(fileSystem);
                }


            }
            else
            {

                createFileSystem(fileSystem);
            }


            // Handle when your app starts

        }

        public static async Task<List<string>> getMostRecentComment(List<Club> mememberClubs)
        {
            var commentTextList = new List<string>();
            for (int i =0; i<mememberClubs.Count; i++)
            {
               var comments = await App.dbWrapper.GetComments(mememberClubs[i].Id);
                if (comments.Count > 0) {
                    var authAccount = await App.dbWrapper.GetAccount(comments[comments.Count-1].AuthorId);
                    string commentText = comments[comments.Count - 1].Text;
                    if(commentText.Length > 30)
                    {
                        commentText = commentText.Substring(0, 30) + "...";
                    }
                    string comment = authAccount.Username + " said: " + commentText;
                    commentTextList.Add(comment);
                }
                else
                {
                    commentTextList.Add("Most Recent Comment...");
                }
            }
            return commentTextList;
        }
        public async void createFileSystem(IFolder fileSystem)
        {
            var loc = await App.dbWrapper.GetLocation();
            var cloudList = await App.dbWrapper.GetAvailableClouds(loc[0], loc[1]);
            string cloudNamesString = "";
            for(int i =0; i<cloudList.Count; i++)
            {
                cloudNamesString +=(cloudList[i].Title) + ",";
                System.Diagnostics.Debug.WriteLine(cloudList[i].Title);

            }
            IFile file = await fileSystem.CreateFileAsync("PhoneData.txt", CreationCollisionOption.ReplaceExisting);
            string baseString = "a\nUVA,"+cloudNamesString+"\n";   ///GOTO: settings page if this is modified
            await file.WriteAllTextAsync(baseString);
            var navPage = new NavigationPage(new CarouselTutorialPage());
            MainPage = navPage;
        }
        public async void createCleanFileSystem(IFolder fileSystem)
        {

            IFile file = await fileSystem.CreateFileAsync("PhoneData.txt", CreationCollisionOption.ReplaceExisting);
            string baseString = "a\nUVA,\n";   ///GOTO: settings page if this is modified
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
