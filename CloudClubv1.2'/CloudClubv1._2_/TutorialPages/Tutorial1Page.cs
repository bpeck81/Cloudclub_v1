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
    public class Tutorial1Page : ContentPage
    {
        public Tutorial1Page()
        {
            //Michael's debug function
          //  DebugDatabase();


            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);
            //  Title = "";
            ColorHandler ch = new ColorHandler();
            BackgroundColor = ch.fromStringToColor("purple");
            
            Image cloudImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Cloud.png"),
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Center,
                Scale = .7
            };
          

            Label headerLabel = new Label
            {
                Text = "Welcome To CloudClub",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 42,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Roboto", WinPhone: "Comic Sans MS"),
                TextColor = Color.White                
            };
            Label informerLabel = new Label
            {
                Text = "The newest innovation in dynamic group chatting",
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.White
            };

         
            Content = new StackLayout
            {
                Children = {
                    cloudImage,
                    headerLabel,
                    informerLabel
                },
                BackgroundColor = Color.FromRgb(210,61,235),
                Spacing = 50,
                Padding = new Thickness(20,0,20,20),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
        }
        
        private async void DebugDatabase(){
            string debug = "MYDEBUG-----";

            //await App.dbWrapper.CreateCloud("Wahoos","wahoo was",38.03,-78.500,.00001);
            //await App.dbWrapper.CreateCloud("Wahoos3", "wahoo was", 38.03, -78.500, .02);

            double[] array = await App.dbWrapper.GetLocation();
            var clouds = await App.dbWrapper.GetAvailableClouds(array[0],array[1]);
            
            foreach(Cloud c in clouds){
                System.Diagnostics.Debug.WriteLine(c.Title);
            }

            await App.dbWrapper.LoginAccount("g","g");
            var user = App.dbWrapper.GetUser();
            var club = (await App.dbWrapper.GetClubs())[0];
            await App.dbWrapper.JoinClubByInvite(user.Id,club.Id);
            await App.dbWrapper.CreateComment("first comment nl",club.Id);
            System.Diagnostics.Debug.WriteLine((await App.dbWrapper.GetRecentComment(club.Id)).Text);

            //await App.dbWrapper.CreateCloud("title","desc",1,2,3);
            //await App.dbWrapper.JoinCloud((await App.dbWrapper.GetClouds())[0].Id);
            
        }

    }
}
