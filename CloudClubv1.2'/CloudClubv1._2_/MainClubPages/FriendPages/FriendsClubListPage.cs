using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using Backend;

namespace FrontEnd
{
    public class FriendsClubListPage : ContentPage
    {
        List<FrontClub> clubList, mutualClubList;
        List<FrontFriendClub> frontFriendClubList;
        List<bool> pendingList;
        ColorHandler ch;
        public FriendsClubListPage(List<Club> clubList, List<Club> mutualClubList, List<bool> pendingList)
        {
            ch = new ColorHandler();
            frontFriendClubList = new List<FrontFriendClub>();
            for(int i =0; i<clubList.Count; i++)
            {
                bool mutualBool=false, joinBool = false, requestBool =false;
                if (mutualClubList.Contains(clubList[i]))
                {
                    mutualBool = true;
                }
                else if(pendingList[i] == true)
                {
                    requestBool = true;
                }
                else
                {
                    joinBool = true;
                }
                  
                
                frontFriendClubList.Add(new FrontFriendClub(clubList[i], mutualBool, joinBool, requestBool, pendingList[i]));
            }

            var listView = new ListView
            {
                ItemsSource = frontFriendClubList,
                ItemTemplate = new DataTemplate(typeof(FriendClubListPageViewCell)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ch.fromStringToColor("white"),
                SeparatorColor = ch.fromStringToColor("gray"),
                HasUnevenRows = true
                            
            };
            Content = listView;
        }
    }
}
