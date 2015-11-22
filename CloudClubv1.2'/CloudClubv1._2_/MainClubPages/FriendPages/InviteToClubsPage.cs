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
    public class InviteToClubsPage : ContentPage
    {
        ListView listView;
        List<FrontInviteToClubFriend> clubList;
        Account friend;
        ColorHandler ch;
        public InviteToClubsPage(List<Club> userClubList, List<Club> mutualClubList, List<Club> pendingList, Account friend)
        {
            ch = new ColorHandler();
            this.friend = friend;
            clubList = new List<FrontInviteToClubFriend>();
            for(int i =0; i <userClubList.Count; i++)
            {
                bool mutualClubBool = false, inviteBool = false, pendingInviteBool = false;
                if (mutualClubList.Contains(userClubList[i])) mutualClubBool = true;
                else if (pendingList.Contains(userClubList[i])) pendingInviteBool = true;
                else
                {
                    inviteBool = true;
                }
                clubList.Add(new FrontInviteToClubFriend(userClubList[i], mutualClubBool, inviteBool, pendingInviteBool, this.friend.Id));
            }
            
            this.updateContent();

        }
        private void updateContent()
        {
            listView = new ListView
            {
                ItemsSource = clubList,
                ItemTemplate = new DataTemplate(typeof(InviteToClubViewCell)),
                BackgroundColor = ch.fromStringToColor("white"),
                SeparatorColor = ch.fromStringToColor("gray"),
                HorizontalOptions = LayoutOptions.FillAndExpand, 
                HasUnevenRows = true,  
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Content = listView;
        }
    }
}
