using System;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd;
using Backend;
using CloudClubv1._2_;
using UIKit;

namespace CloudClubv1._2_.iOS
{
	public class ApnPushes
	{
		public ApnPushes (){}

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
				var alert = new UIAlertView("Cloudclub", "People have been talking in " + club.Title + ".", null, "OK", null);
				alert.Show();
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
				var alert = new UIAlertView("Cloudclub", "People have been talking in " + club.Title + ".", null, "OK", null);
				alert.Show();
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
			var alert = new UIAlertView("Cloudclub", parsedMessage[1], null, "OK", null);
			alert.Show();
		}

		public async Task DropletPush(string[] parsedMessage)
		{
			Comment comment = await DBWrapper.commentTable.LookupAsync(parsedMessage[1]);
			var alert = new UIAlertView("Cloudclub", "Your comment \""+comment.Text+"\" has received "+comment.NumDroplets+" droplets!", null, "OK", null);
			alert.Show();
		}

		public void DBNotificationPush(string[] parsedMessage)
		{
			var alert = new UIAlertView("Cloudclub", parsedMessage[1], null, "OK", null);
			alert.Show();
		}

	}
}

