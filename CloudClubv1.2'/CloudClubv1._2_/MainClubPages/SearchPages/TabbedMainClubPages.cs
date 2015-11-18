using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using CloudClubv1._2_;
using Backend;
using Xamarin.Forms;

namespace FrontEnd
{
    public class TabbedMainClubPages : MyTabbedPage
    {
        ColorHandler ch;
        public string var;
        ClubSearchPage csp;
        MyClubsPage mcp;
        ProfilePage pp;
        public TabbedMainClubPages(List<Club> clubList, List<Club> memberClubList, List<Club> popularClubs, List<Club> newestClubs, List<string> pendingInviteList, List<string> firstLineCommentList)
        {
            ch = new ColorHandler();
            BackgroundColor = ch.fromStringToColor("purple");
            

            // BarTintColor = ch.fromStringToColor("purple");
            NavigationPage.SetHasNavigationBar(this, true);
            // NavigationPage.SetTitleIcon(CurrentPage,"CloudIcon.png");


            this.ToolbarItems.Add(new ToolbarItem
            {
                Icon = "Settings_Top.png",


                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => menuPopup()),

            });
            csp = new ClubSearchPage(clubList, memberClubList, popularClubs, newestClubs, pendingInviteList, firstLineCommentList);
            mcp = new MyClubsPage(memberClubList, firstLineCommentList);
            pp = new ProfilePage();
            csp.Padding = 1;

            mcp.Padding = 1;
            pp.Padding = 1;
            this.Children.Add(csp);
            this.Children.Add(mcp);
            this.Children.Add(pp);
            this.Title = "Explore";


            CurrentPageChanged += TabbedMainClubPages_CurrentPageChanged;

        }

        private async void TabbedMainClubPages_CurrentPageChanged(object sender, EventArgs e)
        {

            if (CurrentPage == csp)
            {
                this.Title = csp.title;
                csp.updateData();
            }
            else if (CurrentPage == mcp)
            {
                this.Title = mcp.title;
                mcp.updateData();
                
            }
            else if (CurrentPage == pp)
            {
                this.Title = pp.title;
                pp.friendRequests = await App.dbWrapper.GetFriendRequests();
                pp.medals = await App.dbWrapper.GetMedals();

            }
        }


        private void menuPopup()
        {
            Navigation.PushAsync(new SettingsPage());
        }
    }
}
