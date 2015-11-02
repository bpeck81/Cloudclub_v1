using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using FrontEnd;
using Backend;
using CloudClubv1._2_;
using System.Threading.Tasks;

namespace CloudClubv1._2_.Droid
{
    class GcmPushes
    {
        private GcmService GService;

        public GcmPushes(GcmService gcmService) {
            GService = gcmService;
        }

        public async Task CommentPush(string[] parsedMessage) {
            var comment = await DBWrapper.commentTable.LookupAsync(parsedMessage[1]);
            var club = await DBWrapper.clubTable.LookupAsync(comment.ClubId);

            //if the comment is in the club the user is currently in, add it to the currentCommentList
            if (club.Id.Equals(DBWrapper.CurrentClubId))
            {
                var account = await DBWrapper.accountTable.LookupAsync(comment.AuthorId);
                ClubChatPage.CurrentCommentsList.Add(new FrontComment(comment, account));
                return;

                //if the comment is not in the current opend club, make a push notification
            }
            else
            {
                GService.createNotification("Cloudclub", "People have been talking in " + club.Title + ".");
                return;
            } 
        }

        public async Task LikePush(string[] parsedMessage) {
            var dbComment = await DBWrapper.commentTable.LookupAsync(parsedMessage[1]);

            //hacky solution; i should implement the interface inotifypropertychanged https://msdn.microsoft.com/en-us/library/vstudio/ms743695(v=vs.100).aspx
            var comment = ClubChatPage.CurrentCommentsList.FirstOrDefault(item => item.Id == dbComment.Id);
            comment.NumDroplets = dbComment.NumDroplets;
            int index = ClubChatPage.CurrentCommentsList.IndexOf(comment);
            ClubChatPage.CurrentCommentsList.Remove(comment);
            ClubChatPage.CurrentCommentsList.Insert(index, comment);


            System.Diagnostics.Debug.WriteLine("COMMENT LIKED");

            return;
        }

        public void MedalPush(string[] parsedMessage)
        {
            GService.createNotification("Cloudclub", parsedMessage[1]);
        }

        public async Task DropletPush(string[] parsedMessage)
        {
            Comment comment = await DBWrapper.commentTable.LookupAsync(parsedMessage[1]);
            GService.createNotification("Cloudclub", "Your comment \""+comment.Text+"\" has received "+comment.NumDroplets+" droplets!");
        }

        public void DBNotificationPush(string[] parsedMessage)
        {
            GService.createNotification("Cloudclub", parsedMessage[1]);
        }

    }
}