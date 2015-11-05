﻿using System;
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
            DebugDatabase();


            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);
            //  Title = "";
            
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
            await App.dbWrapper.CreateAccount("252","252");
            await App.dbWrapper.LoginAccount("252","252");
            System.Diagnostics.Debug.WriteLine(debug+App.dbWrapper.GetUser().Username);
            await App.dbWrapper.CreateClub("title","blue",false,new List<string>());
            var club = (await App.dbWrapper.GetClubs())[0];
            System.Diagnostics.Debug.WriteLine(debug+club.Title);

            await App.dbWrapper.CreateComment(",yo",club.Id);
           // await App.dbWrapper.CreateComment(",yo", club.Id);
           // await App.dbWrapper.LoginAccount("22", "22");

            //var user = App.dbWrapper.GetUser();
            
           // await App.dbWrapper.SetCurrentClubId(club.Id);
            //await App.dbWrapper.RemoveCurrentClubId();
            
        }

    }
}
