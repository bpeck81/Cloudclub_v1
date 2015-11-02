using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontEnd;
using Backend;

using Xamarin.Forms;

namespace CloudClubv1._2_
{
    public class App : Application
    {
        public static DBWrapperInterface dbWrapper;


        public App(DBWrapperInterface myDBWrapper)
        {
            // The root page of your application
            dbWrapper = myDBWrapper;
            ColorHandler ch = new ColorHandler();
           var navPage = new NavigationPage(new CarouselTutorialPage());
            navPage.BarBackgroundColor = ch.fromStringToColor("purple");
            MainPage = navPage;

        }


        protected override async void OnStart()
        {
            
            // Handle when your app starts
            
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
