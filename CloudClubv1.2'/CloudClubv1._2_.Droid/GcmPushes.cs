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

//NOTE: sometimes pushes are tricky to debug since they come after the code has executed

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
                //add the club to a list of active clubs the user needs to still check
                if(!DBWrapper.ActiveClubs.Contains(club.Id)){
                    DBWrapper.ActiveClubs.Add(club.Id);
                }

                //make notification
                GService.createNotification("Cloudclub", "People have been talking in " + club.Title + ".");
                return;
            } 
        }

        public async Task ClubRequestPush(string[] parsedMessage)
        {
            var clubRequest = await DBWrapper.clubRequestTable.LookupAsync(parsedMessage[1]);
            var club = await DBWrapper.clubTable.LookupAsync(clubRequest.ClubId);

            //if the comment is in the club the user is currently in, add it to the currentCommentList
            if (club.Id.Equals(DBWrapper.CurrentClubId))
            {

                var account = await DBWrapper.accountTable.LookupAsync(clubRequest.AccountId);

                //TODO: MAKE FRONTCLUBREQUEST
                //ClubChatPage.CurrentCommentsList.Add(new FrontComment(comment, account));
                return;

                //if the comment is not in the current opend club, make a push notification
            }
            else
            {
                //add the club to a list of active clubs the user needs to still check
                if (!DBWrapper.ActiveClubs.Contains(club.Id))
                {
                    DBWrapper.ActiveClubs.Add(club.Id);
                }

                //make notification
                GService.createNotification("Cloudclub", "People have been talking in " + club.Title + ".");
                return;
            }
        }

        public async Task LikePush(string[] parsedMessage) {
            var dbComment = await DBWrapper.commentTable.LookupAsync(parsedMessage[1]);

            var comment = ClubChatPage.CurrentCommentsList.FirstOrDefault(item => item.Id == dbComment.Id);
            comment.NumDroplets = dbComment.NumDroplets;

            //the frontend comment implements inotifypropertychanged so it updates on property changed
            comment.UpdateProperty("NumDroplets");

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