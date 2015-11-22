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
    public class NewsPage : ContentPage
    {
        ColorHandler ch;
        List<FrontNews> frontNewsList;
        public NewsPage(List<DBItem> newsItems, string friendRequestUsername)
        {
            ch = new ColorHandler();
            frontNewsList = new List<FrontNews>();
            generateFrontNewsList(newsItems, friendRequestUsername);
            updatePage();
            Title = "News";

        }

        private void updatePage()
        {
            var listView = new ListView
            {
                ItemsSource = frontNewsList,
                ItemTemplate = new DataTemplate(typeof(NewsPageViewCell)),
                BackgroundColor = ch.fromStringToColor("white"),
                HasUnevenRows = true

            };
            listView.ItemTapped += ListView_ItemSelected;

            Content = listView;

        }

        private async void ListView_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            var news = (FrontNews)e.Item;
            if (news.NotificationType == "friendRequest")
            {
                FriendRequest request = (FriendRequest)news.dbItem;
                var authAcc = await App.dbWrapper.GetAccount(request.AuthorId);
                
                await Navigation.PushAsync(new FriendProfilePage(authAcc, 1, request));

            }
            else if (news.NotificationType == "warning")
            {

            }
            else if (news.NotificationType == "invite")
            {
                // var club = 
                var invite = (Invite)news.dbItem;

                var chatList = await App.dbWrapper.GetChat(invite.ClubId, "","");

                List<Account> requestUsersList = new List<Account>();
                List<Account> commentUsersList = new List<Account>();

                for (int i = 0; i < chatList.Count; i++)
                {
                    if (chatList[i].GetType() == typeof(Comment))
                    {
                        var comment = (Comment)chatList[i];
                        commentUsersList.Add(await App.dbWrapper.GetAccount(comment.AuthorId));

                    }
                    else if (chatList[i].GetType() == typeof(ClubRequest))
                    {
                        var request = (ClubRequest)chatList[i];
                        requestUsersList.Add(await App.dbWrapper.GetAccount(request.AccountId));
                    }
                }

                bool isMember = await App.dbWrapper.IsMember(invite.ClubId);
                var club = await App.dbWrapper.GetClub(invite.ClubId);
                await App.dbWrapper.SetCurrentClubId(invite.ClubId);

                await Navigation.PushAsync(new ClubChatPage(new FrontClub(club, false, false), chatList, commentUsersList, requestUsersList, isMember));



            }

        }

        private void generateFrontNewsList(List<DBItem> newsItems, string friendRequestUsername)
        {
            for (int i = 0; i < newsItems.Count; i++)
            {

                if (newsItems[i].GetType() == typeof(Medal))
                {
                    var medal = (Medal)newsItems[i];
                    frontNewsList.Add(new FrontNews("medal", medal.MedalName, newsItems[i].Time, newsItems[i]));

                }
                else if (newsItems[i].GetType() == typeof(DBNotification))
                {
                    DBNotification notification = (DBNotification)newsItems[i];
                    switch (notification.Type)
                    {
                        case "droplet":
                            frontNewsList.Add(new FrontNews("droplet", notification.Text, newsItems[i].Time, newsItems[i]));
                            break;
                        case "rank":
                            frontNewsList.Add(new FrontNews("rank", notification.Text, newsItems[i].Time, newsItems[i]));
                            break;
                        case "warning":
                            frontNewsList.Add(new FrontNews("warning", notification.Text, newsItems[i].Time, newsItems[i]));

                            break;
                        case "join":
                            frontNewsList.Add(new FrontNews("join", notification.Text, newsItems[i].Time, newsItems[i]));
                            break;
                        case "friend":
                            frontNewsList.Add(new FrontNews("friend", notification.Text, newsItems[i].Time, newsItems[i]));
                            break;
                        case "ban":
                            frontNewsList.Add(new FrontNews("ban", notification.Text, newsItems[i].Time, newsItems[i]));
                            break;
                        case "clubReport":
                            frontNewsList.Add(new FrontNews("clubReport", notification.Text, newsItems[i].Time, newsItems[i]));
                            break;
                    }


                }
                else if (newsItems[i].GetType() == typeof(ClubRequest))
                {
                    var item = (ClubRequest)newsItems[i];
                    frontNewsList.Add(new FrontNews("clubRequest", item.Text, newsItems[i].Time, newsItems[i]));

                }
                else if (newsItems[i].GetType() == typeof(Invite))
                {

                    var item = (Invite)newsItems[i];
                    frontNewsList.Add(new FrontNews("invite", item.ClubId, newsItems[i].Time, newsItems[i]));

                }
                else if (newsItems[i].GetType() == typeof(FriendRequest))
                {
                    var item = (FriendRequest)newsItems[i];
                    frontNewsList.Add(new FrontNews("friendRequest", friendRequestUsername, newsItems[i].Time, newsItems[i]));
                }

            }
        }
    }
}
