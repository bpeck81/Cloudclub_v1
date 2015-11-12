﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using CloudClubv1._2_;
using Backend;

using Xamarin.Forms;

namespace FrontEnd
{
    public class TabbedMainClubPages : TabbedPage
    {
        ColorHandler ch;
        public string var;
        ClubSearchPage csp;
        MyClubsPage mcp;
        ProfilePage pp;
        public TabbedMainClubPages(List<Club> clubList, List<Club> memberClubList, List<Club> popularClubs, List<Club> newestClubs, List<string> pendingInviteList)   
        {
            ch = new ColorHandler();
            
            NavigationPage.SetHasNavigationBar(this, true);
           // NavigationPage.SetTitleIcon(CurrentPage,"CloudIcon.png");
            this.BackgroundColor = ch.fromStringToColor("purple");

            this.ToolbarItems.Add(new ToolbarItem
            {
                Icon = "Settings_Top.png",

                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => menuPopup())
            });
            csp = new ClubSearchPage(clubList, memberClubList, popularClubs,newestClubs,pendingInviteList);
            mcp = new MyClubsPage(memberClubList);
            pp = new ProfilePage();
            this.Children.Add(csp);
            this.Children.Add(mcp);
            this.Children.Add(pp);
            this.Title = "Explore";

            this.CurrentPageChanged += TabbedMainClubPages_CurrentPageChanged;

        }

        private async void TabbedMainClubPages_CurrentPageChanged(object sender, EventArgs e)
        {

            if (CurrentPage == csp) { this.Title = csp.title; }
            else if (CurrentPage == mcp) { this.Title = mcp.title; }
            else if (CurrentPage == pp) {
                this.Title = pp.title;
                pp.friendRequests= await App.dbWrapper.GetFriendRequests();
                pp.medals = await App.dbWrapper.GetMedals();
                
               
                ///await App.dbWrapper.get
                             

            }
        }


        private void menuPopup()
        {
            Navigation.PushAsync(new SettingsPage());
        }
    }
}