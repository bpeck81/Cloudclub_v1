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
    public class InviteFriendsPage : ContentPage
    {
        ListView listView;
        List<FrontClubMember> friendsList;
        public InviteFriendsPage(List<FrontClubMember> friendsList)
        {
            this.friendsList = friendsList;
            this.updateContent(); 
            
        }
        private void updateContent()
        {
            listView = new ListView
            {
                ItemsSource = friendsList,
                ItemTemplate = new DataTemplate(typeof(inviteFriendsViewCell))
            };
        }
    }
}
