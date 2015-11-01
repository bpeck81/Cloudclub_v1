using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;

namespace CloudClubv1._2_
{
    public interface DBWrapperInterface
    {
          Task<bool> CreateAccount(string username, string password);
          Task<bool> LoginAccount(string username, string password);
          Task<string> SetUserEmoji(string emoji);
          Task<string> SetUserColor(string color);
          Task<bool> CreateClub(string title, string color, bool exclusive, List<string> tagList);
          Task<List<Club>> GetClubs();
          Task<int> RateClub(int rating, string clubId);
          Task<int> JoinClub(string clubId);
          Task<string> CreateContactUs(string text);
          Task<int> CreateComment(string text, string clubId);
          Task<List<Comment>> GetComments(string clubId);
          Task<int> RateComment(string commentId);
          Task<List<Account>> GetAccounts();
          Task<Account> GetAccount(string accountId);
          Task<bool> CreateFriendRequest(string recipientId);
          Task<List<FriendRequest>> GetFriendRequests();
          Task<bool> DeclineFriendRequest(string friendRequestId);
          Task<bool> AcceptFriendRequest(string friendRequestId);
          Task<string> CreateDBMessage(string text, string recipientId);
          Task<List<DBMessage>> GetDBMessages();
          Task<bool> CreateInvite(string clubId, string recipientId);
          Task<List<Invite>> GetInvites();
          Task<int> JoinClubByInvite(string accountId, string clubId);
          Task<bool> AcceptInvite(string inviteId);
          Task<bool> DeclineInvite(string inviteId);
          Task<bool> CreateClubRequest(string text, string clubId);
          Task<List<ClubRequest>> GetClubRequests(string clubId);
          Task<bool> AcceptClubRequest(string clubRequestId);
          Task<bool> DeclineClubRequest(string clubRequestId);
          Task<List<Comment>> ApiCall();
          Task<List<Club>> GetNewestClubs();
          Task<List<Club>> SearchClubs(List<string> tags);
          Task<string> CreateDBImage(string path, string clubId);
          Task<List<DBImage>> GetDBImages();
          Task<bool> IsMember(string clubId);
          Task<List<Club>> GetPopularClubs();
          Task<List<Tag>> GetTags(string clubId);
          Task<List<Club>> GetActiveClubs();
          Task<List<DBItem>> GetNewsFeed();
          Task<string> CreateMedal(string medalName, int points) ;
          Task<List<Medal>> GetMedals();
          Task<DBNotification> TestDBNotification();
          Task<string> CreateBan(string accountId, string commentId);
          Account GetUser();
          string GetCurrentClub();
          void LogoutUser();
          Task<int> GetFriendship(string accountId);
          Task<int> GetUserRating(string clubId);
          Task<bool> LeaveClub(string clubId);
          Task<List<Account>> GetFriends(string accountId);
          Task<int> GetSharedFriendsCount(string accountId);
          Task<bool> InSameClub(string accountId);
          Task<bool> IsPendingClubRequest(string clubId);
          Task<bool> IsPendingInvite(string clubId,string accountId);
          Task<bool> IsClubMember(string clubId, string accountId);
          Task<bool> HasRatedComment(string accountId, string commentId);
          Task<bool> DeleteTag(string tag, string clubId);
    }
}