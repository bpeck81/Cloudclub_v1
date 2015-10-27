using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CloudClubv1._2_;
using Backend;

namespace CloudClubv1._2_.Droid
{
    public class DBWrapper:DBWrapperInterface
    {
        //INITIALIZATION

        //initialize client
        public static MobileServiceClient client = new MobileServiceClient(
            "https://michaelmwhitemb.azure-mobile.net/",
            "ZtXpkKwGzzANTnwRnqrcwiKHqfXTBY18"
        );

        //initialize tables
        public static IMobileServiceTable<Account> accountTable = client.GetTable<Account>();
        public static IMobileServiceTable<Club> clubTable = client.GetTable<Club>();
        public static IMobileServiceTable<Tag> tagTable = client.GetTable<Tag>();
        public static IMobileServiceTable<RatingJunction> ratingJunctionTable = client.GetTable<RatingJunction>();
        public static IMobileServiceTable<ContactUs> contactUsTable = client.GetTable<ContactUs>();
        public static IMobileServiceTable<MemberJunction> memberJuncTable = client.GetTable<MemberJunction>();
        public static IMobileServiceTable<Comment> commentTable = client.GetTable<Comment>();
        public static IMobileServiceTable<CommentJunction> commentJuncTable = client.GetTable<CommentJunction>();
        public static IMobileServiceTable<FriendRequest> friendRequestTable = client.GetTable<FriendRequest>();
        public static IMobileServiceTable<Friends> friendsTable = client.GetTable<Friends>();
        public static IMobileServiceTable<DBMessage> DBMessageTable = client.GetTable<DBMessage>();
        public static IMobileServiceTable<Invite> inviteTable = client.GetTable<Invite>();
        public static IMobileServiceTable<ClubRequest> clubRequestTable = client.GetTable<ClubRequest>();
        public static IMobileServiceTable<DBImage> DBImageTable = client.GetTable<DBImage>();
        public static IMobileServiceTable<Medal> medalTable = client.GetTable<Medal>();
        public static IMobileServiceTable<DBNotification> DBNotificationTable = client.GetTable<DBNotification>();
        public static IMobileServiceTable<Ban> banTable = client.GetTable<Ban>();

        //class members
        private Account User;

        //constants
        private const int CLUB_SIZE = 50;


        public DBWrapper()
        {
            
            //initialize mobile services
            CurrentPlatform.Init();
        }


        //FUNCTIONS

        ///Returns the user; empty if not logged in
        public Account GetUser() {
            return User;
        }

        /// Create an account; returns 1 if successful, 0 if username is already in use
        public async Task<bool> CreateAccount(string username, string password)
        {
            Account account = new Account(username, password);

            //check if username in use
            var list = await accountTable.Where(item => item.Username == account.Username).ToListAsync();

            if (list.Count > 0)
            {
                return false;
            }
            else
            {
                await accountTable.InsertAsync(account);
                return true;
            }

        }

        /// Login to an account; returns 1 if successful, 0 if failure
        public async Task<bool> LoginAccount(string username, string password)
        {
            var list = await accountTable.Where(item => item.Username == username && item.Password == password).ToListAsync();

            if (list.Count > 0)
            {
                User = list[0];
                return true;
            }
            else
            {
                return false;
            }

        }

        //NOTE: FUNCTIONS FROM HERE ON ASSUME THAT THE USER IS LOGGED IN AS A PRECONDITION

        /// Set the user's emoji; returns the user's emoji
        public async Task<string> SetUserEmoji(string emoji)
        {
            User.Emoji = emoji;
            await accountTable.UpdateAsync(User);

            return User.Emoji;
        }

        /// Set the user's color; returns the user's color
        public async Task<string> SetUserColor(string color)
        {
            User.Color = color;
            await accountTable.UpdateAsync(User);

            return User.Color;
        }

        /// Create a club; pass 0 for a public club and 1 for an exclusive club; returns if club was created, fails if 
        /// the club name is already in use
        public async Task<bool> CreateClub(string title, string color, bool exclusive, List<string> tagList)
        {
            Club club = new Club(title, color, exclusive, User.Id);

            //check if the club name is in use
            var list = await clubTable.Where(item => item.Title == club.Title).ToListAsync();

            if (list.Count > 0)
            {
                return false;
            }
            else
            {
                await clubTable.InsertAsync(club);

                //add tags to table
                foreach (string key in tagList)
                {
                    Tag tag = new Tag(key, club.Id);
                    await tagTable.InsertAsync(tag);
                }

                //add club founder to memberJunction
                //club size is initialized to one automatically
                MemberJunction memberJunction = new MemberJunction(User.Id, club.Id);
                await memberJuncTable.InsertAsync(memberJunction);

                //update user account
                User.NumClubsCreated++;
                User.NumClubsIn++;
                User.Points++;
                await accountTable.UpdateAsync(User);

                return true;
            }

        }

        /// Get clubs; returns a list of club instances
        /// TODO: sort by popularity
        public async Task<List<Club>> GetClubs()
        {
            List<Club> list = await clubTable.ToListAsync();
            return list;
        }

        /// Rate a club; returns what the user has rated the club
        /// as of now, anyone can rate a club, but perhaps only members should be able to rate clubs
        public async Task<int> RateClub(int rating, string clubId)
        {
            Club club = await clubTable.LookupAsync(clubId);

            //check if user has rated club
            var list = await ratingJunctionTable.Where(item => item.AccountId == User.Id && item.ClubId == club.Id).ToListAsync();

            if (list.Count > 0)
            {
                RatingJunction ratingJunction = list[0];

                //remove old rating from club
                club.TotalRating -= ratingJunction.Rating;
                ratingJunction.Rating = rating;

                //add new rating to club
                club.TotalRating += ratingJunction.Rating;
                await ratingJunctionTable.UpdateAsync(ratingJunction);

            }
            else
            {
                RatingJunction ratingJunction = new RatingJunction(rating, User.Id, clubId);
                await ratingJunctionTable.InsertAsync(ratingJunction);

                club.TotalRating += ratingJunction.Rating;
                club.NumRatings += 1;
            }
            await clubTable.UpdateAsync(club);

            return rating;
        }

        /// Try to join a club; returns the status of trying to join the club as an int:
        /// 1 - success, 2 - club full, 3 - club private, 4 - already in club
        public async Task<int> JoinClub(string clubId)
        {
            //assumes the club exists as a precondition
            Club club = await clubTable.LookupAsync(clubId);

            if (club.NumMembers >= CLUB_SIZE)
            {
                return 2;
            }
            if (club.Exclusive == true)
            {
                return 3;
            }
            var list = await memberJuncTable.Where(item => item.AccountId == User.Id && item.ClubId == club.Id).ToListAsync();
            if (list.Count > 0)
            {
                return 4;
            }

            //assuming all precondition do not prevent user from joining club
            MemberJunction memberJunc = new MemberJunction(User.Id, club.Id);
            await memberJuncTable.InsertAsync(memberJunc);

            //update club
            club.NumMembers++;
            await clubTable.UpdateAsync(club);

            //update user
            User.NumClubsIn++;
            await accountTable.UpdateAsync(User);

            //make DBNotification
            DBNotification DBNotification = new DBNotification(User.Id,"join","You joined the club "+club.Title+"!");
            await DBNotificationTable.InsertAsync(DBNotification);

            return 1;

        }

        ///Create a contact us form; returns the text that the user writes
        public async Task<string> CreateContactUs(string text)
        {
            ContactUs contactUs = new ContactUs(User.Id, text);
            await contactUsTable.InsertAsync(contactUs);

            return contactUs.Text;
        }

        ///Create a comment; returns 1 if success, 2 if not a member of the club, 3 if banned
        public async Task<int> CreateComment(string text, string clubId)
        {
            //check if banned (must get current instance of user account incase ban happend since login)
            User = await accountTable.LookupAsync(User.Id);
            if(DateTime.Compare(User.Banned,DateTime.Now)>0){
                return 3;
            }

            Club club = await clubTable.LookupAsync(clubId);

            //check if user is in club
            var list = await memberJuncTable.Where(item => item.AccountId == User.Id && item.ClubId == club.Id).ToListAsync();

            if (list.Count > 0)
            {
                Comment comment = new Comment(User.Id, clubId, text);
                await commentTable.InsertAsync(comment);

                //update user
                User.NumComments++;
                await accountTable.UpdateAsync(User);

                return 1;
            }
            else
            {
                return 2;
            }

        }

        /// Get comments in a club; returns a list of comment instances
        /// TODO: sort by date and time
        public async Task<List<Comment>> GetComments(string clubId)
        {
            List<Comment> list = await commentTable.Where(item => item.ClubId == clubId).ToListAsync();
            return list;
        }

        /// Add a droplet to a comment; returns status as an int:
        /// 1 - successfully rated comment; 2 - comment already rated
        /// TODO: unlike a comment?
        /// TODO: anyone can like a comment, even a non club member - i think this is a good idea but not entirely sure
        public async Task<int> RateComment(string commentId)
        {
            var list = await commentJuncTable.Where(item => item.RaterId == User.Id && item.CommentId == commentId).ToListAsync();

            if (list.Count > 0)
            {
                return 2;
            }
            else
            {
                //add rating junction
                CommentJunction commentJunction = new CommentJunction(User.Id, commentId);
                await commentJuncTable.InsertAsync(commentJunction);

                //update comment with rating
                Comment comment = await commentTable.LookupAsync(commentId);
                comment.NumDroplets++;
                await commentTable.UpdateAsync(comment);

                //update author with rating
                Account author = await accountTable.LookupAsync(comment.AuthorId);
                author.NumDroplets++;
                author.Points++;
                await accountTable.UpdateAsync(author);

                return 1;
            }

        }

        /// Get a list of all accounts; returns list of account instances
        public async Task<List<Account>> GetAccounts()
        {
            List<Account> list = await accountTable.ToListAsync();
            return list;
        }

        /// Get a single account; returns an account instance
        public async Task<Account> GetAccount(string accountId)
        {
            Account account = await accountTable.LookupAsync(accountId);
            return account;
        }

        /// Make a friend request to another user; returns true if request is made, false if one exists already
        /// TODO: what if a user who has a pending fr req sends one to someone who already sent him one? rows flipped?
        /// TODO: make sure you can't friend yourself? is that even an option?
        public async Task<bool> CreateFriendRequest(string recipientId)
        {
            var list = await friendRequestTable.Where(item => item.AuthorId == User.Id && item.RecipientId == recipientId).ToListAsync();

            if (list.Count > 0)
            {
                return false;
            }
            else
            {
                FriendRequest friendRequest = new FriendRequest(User.Id, recipientId);
                await friendRequestTable.InsertAsync(friendRequest);

                return true;
            }
        }

        /// Returns pending friend requests the user has
        /// TODO: organize by date and combine with other account updates
        /// TODO: frontend needs a way of getting a list of user instances who make comments and friend requests
        public async Task<List<FriendRequest>> GetFriendRequests()
        {
            var list = await friendRequestTable.Where(item => item.RecipientId == User.Id).ToListAsync();
            return list;
        }

        /// Deletes a friend request; returns true if successful, false if not
        public async Task<bool> DeclineFriendRequest(string friendRequestId)
        {
            try
            {
                FriendRequest friendRequest = await friendRequestTable.LookupAsync(friendRequestId);
                await friendRequestTable.DeleteAsync(friendRequest);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// Makes a friend object in the friends table and deletes the friend request; returns true if success, false if not
        public async Task<bool> AcceptFriendRequest(string friendRequestId)
        {
            FriendRequest friendRequest = await friendRequestTable.LookupAsync(friendRequestId);

            //add to friends table
            Friends friends = new Friends(friendRequest.AuthorId, friendRequest.RecipientId);
            await friendsTable.InsertAsync(friends);

            //update accounts
            Account author = await accountTable.LookupAsync(friends.AuthorId);
            author.NumFriends++;
            await accountTable.UpdateAsync(author);

            Account recipient = await accountTable.LookupAsync(friends.RecipientId);
            recipient.NumFriends++;
            await accountTable.UpdateAsync(recipient);

            //add DBNotifications
            DBNotification DBNotificationAuthor = new DBNotification(author.Id,"friend","You and "+recipient.Username+" became friends!");
            await DBNotificationTable.InsertAsync(DBNotificationAuthor);

            DBNotification DBNotificationRecipient = new DBNotification(recipient.Id, "friend", "You and "+author.Username+" became friends!");
            await DBNotificationTable.InsertAsync(DBNotificationRecipient);

            //delete friendrequest
            try
            {
                await friendRequestTable.DeleteAsync(friendRequest);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }

        }

        /// Send a personal DBMessage to another account; returns text of the DBMessage
        public async Task<string> CreateDBMessage(string text, string recipientId)
        {
            DBMessage DBMessage = new DBMessage(text, User.Id, recipientId);
            await DBMessageTable.InsertAsync(DBMessage);

            return DBMessage.Text;
        }

        /// Get all personal DBMessages for the user; returns a list of DBMessage instances
        public async Task<List<DBMessage>> GetDBMessages()
        {
            List<DBMessage> list = await DBMessageTable.Where(item => item.RecipientId == User.Id).ToListAsync();
            return list;
        }

        /// Send an invite to join a club to an account; returns true if success, false if the account already has an invite
        /// NOTE: you can currently send an invite to someone already in the club
        public async Task<bool> CreateInvite(string clubId, string recipientId)
        {
            var list = await inviteTable.Where(item => item.ClubId == clubId && item.RecipientId == recipientId).ToListAsync();

            //if the user doesn not already have in invite, make one
            if (list.Count > 0)
            {
                return false;
            }
            else
            {
                Invite invite = new Invite(User.Id, recipientId, clubId);
                await inviteTable.InsertAsync(invite);

                //make a DBNotification
                Club club = await clubTable.LookupAsync(clubId);
                DBNotification DBNotification = new DBNotification(recipientId,"invite",User.Username+" has invited you to join "+club.Title+".");
                await DBNotificationTable.InsertAsync(DBNotification);

                return true;
            }
        }

        /// Get all club invites for the user; returns a list of invite instances
        public async Task<List<Invite>> GetInvites()
        {
            List<Invite> list = await inviteTable.Where(item => item.RecipientId == User.Id).ToListAsync();
            return list;
        }

        /// Join a club after being invited; ignores if exclusive and returns the status of trying to join the club as an int:
        /// 1 - success, 2 - club full, 4 - already in club
        public async Task<int> JoinClubByInvite(string accountId, string clubId)
        {
            //assumes the club exists as a precondition
            Club club = await clubTable.LookupAsync(clubId);

            if (club.NumMembers >= CLUB_SIZE)
            {
                return 2;
            }
            var list = await memberJuncTable.Where(item => item.AccountId == accountId && item.ClubId == club.Id).ToListAsync();
            if (list.Count > 0)
            {
                return 4;
            }

            //assuming all precondition do not prevent user from joining club
            MemberJunction memberJunc = new MemberJunction(accountId, club.Id);
            await memberJuncTable.InsertAsync(memberJunc);

            //update club
            club.NumMembers++;
            await clubTable.UpdateAsync(club);

            //update user
            Account account = await accountTable.LookupAsync(accountId);
            account.NumClubsIn++;
            await accountTable.UpdateAsync(account);

            //add DBNotifications
            DBNotification DBNotification = new DBNotification(account.Id,"join","You joined the club "+club.Title+"!");
            await DBNotificationTable.InsertAsync(DBNotification);

            return 1;

        }

        /// join the club and delete the invite; returns true if success, false if failure
        public async Task<bool> AcceptInvite(string inviteId)
        {
            Invite invite = await inviteTable.LookupAsync(inviteId);

            //join the club: NOTE: the DBNotification is made in the joinclubbyinvite function
            await JoinClubByInvite(User.Id, invite.ClubId);

            //delete the invite
            try
            {
                await inviteTable.DeleteAsync(invite);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// delete an invite; returns true if success, false if failure
        public async Task<bool> DeclineInvite(string inviteId)
        {
            Invite invite = await inviteTable.LookupAsync(inviteId);

            try
            {
                await inviteTable.DeleteAsync(invite);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// create a request to join a club; returns true if success, false if the user has already requested to join
        /// TODO: im making it as though a single user votes, but how is it accepted or rejected?f
        public async Task<bool> CreateClubRequest(string text, string clubId)
        {
            var list = await clubRequestTable.Where(item => item.AccountId == User.Id && item.ClubId == clubId).ToListAsync();

            if (list.Count > 0)
            {
                return false;
            }
            else
            {
                ClubRequest clubRequest = new ClubRequest(User.Id, clubId, text);
                await clubRequestTable.InsertAsync(clubRequest);

                return true;
            }
        }

        /// Returns a list of club request instances for a given club
        public async Task<List<ClubRequest>> GetClubRequests(string clubId)
        {
            var list = await clubRequestTable.Where(item => item.ClubId == clubId).ToListAsync();
            return list;
        }

        /// join the club and delete the club request; returns true if success, false if failure
        public async Task<bool> AcceptClubRequest(string clubRequestId)
        {
            ClubRequest clubRequest = await clubRequestTable.LookupAsync(clubRequestId);

            //author joins the club
            await JoinClubByInvite(clubRequest.AccountId, clubRequest.ClubId);

            //make DBNotification
            Club club = await clubTable.LookupAsync(clubRequest.ClubId);
            DBNotification DBNotification = new DBNotification(clubRequest.AccountId,"request",club.Title+" has accepted your request!");
            await DBNotificationTable.InsertAsync(DBNotification);

            //delete the club request
            try
            {
                await clubRequestTable.DeleteAsync(clubRequest);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// delete a club request; returns true if success, false if failure
        public async Task<bool> DeclineClubRequest(string clubRequestId)
        {
            ClubRequest clubRequest = await clubRequestTable.LookupAsync(clubRequestId);

            try
            {
                await clubRequestTable.DeleteAsync(clubRequest);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// An example call to demonstrate using a custom api
        public async Task<List<Comment>> ApiCall()
        {
            var data = await client.InvokeApiAsync("backendapi");
            var table = data["table"];
            var result = table.ToObject<List<Comment>>();
            return result;
        }

        /// Returns a list of the twenty newest clubs to be made
        public async Task<List<Club>> GetNewestClubs()
        {
            List<Club> clubs = await clubTable.OrderByDescending(item => item.Time).Take(20).ToListAsync();
            return clubs;
        }

        /// Get the top 20 clubs that match a search query; pass in a string list of tags and a list of clubs is returned
        public async Task<List<Club>> SearchClubs(List<string> tags)
        {
            List<Club> clubs = await client.InvokeApiAsync<List<string>, List<Club>>("SearchClubs", tags);
            return clubs;

        }

        //NOTE: in the server, the access info is that of the storage account, not the mobile service
        //and it operates on a key value pair, 

        /// Create an DBImage class and send it to the server
        public async Task<string> CreateDBImage(string path, string clubId)
        {
            DBImage DBImage = new DBImage();

            //set properties of the DBImage
            DBImage.ContainerName = "containerName";
            DBImage.ResourceName = Guid.NewGuid().ToString();

            //upload DBImage, the server will give it an sasquerystring
            await DBImageTable.InsertAsync(DBImage);

            //if the server assigned the string
            if (!string.IsNullOrEmpty(DBImage.SasQueryString))
            {
                System.Diagnostics.Debug.WriteLine("SAS STRING WAS MADE ----------------------");
                StorageCredentials credentials = new StorageCredentials(DBImage.SasQueryString);
                var DBImageUri = new Uri(DBImage.DBImageUri);
                System.Diagnostics.Debug.WriteLine("-----------------URI---------------"+DBImageUri.ToString());

                //instatiate blob container given credentials
                CloudBlobContainer container = new CloudBlobContainer(
                    new Uri(string.Format("https://{0}/{1}", DBImageUri.Host, DBImage.ContainerName)), credentials
                    );

                //files are unmanaged resources so they should be used like this
                //a managed resource is one that is handled by the garbage collector
                using (var inputStream = File.OpenRead(path))
                {
                    //upload the DBImage from the file stream
                    CloudBlockBlob blob = container.GetBlockBlobReference(DBImage.ResourceName);
                    await blob.UploadFromStreamAsync(inputStream);
                    System.Diagnostics.Debug.WriteLine("IT ACTUALLY WORKED!!!!!!!!!!!!!!!!!! NO WAY!!!!!!!!------------------");
                }

                //add a comment that references the picture for easy retrieval in clubs: the DBImage's id is stored in the text field
                //NOTE: assumes the user is a member of the club as a precondition
                Club club = await clubTable.LookupAsync(clubId);

                Comment comment = new Comment(User.Id, clubId, DBImage.Id);
                comment.Picture = true;
                await commentTable.InsertAsync(comment);

                //update user
                User.NumComments++;
                await accountTable.UpdateAsync(User);

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SAS STRING NOT MADE-------------------------------");
            }

            return path;
        }

        ///get a list of all of the DBImages in the database
        public async Task<List<DBImage>> GetDBImages()
        {
            List<DBImage> DBImages = await DBImageTable.ToListAsync();
            return DBImages;
        }

        //HERE FROM ON IS WHAT IVE DONE

        ///gets whether a user is the member of a club; returns true if they are, false if not
        public async Task<bool> IsMember(string clubId)
        {
            //assumes the club exists as a precondition
            Club club = await clubTable.LookupAsync(clubId);

            var list = await memberJuncTable.Where(item => item.AccountId == User.Id && item.ClubId == club.Id).ToListAsync();
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// Returns a list of the twenty most rated clubs
        public async Task<List<Club>> GetPopularClubs()
        {
            List<Club> clubs = await clubTable.OrderByDescending(item => item.TotalRating).Take(20).ToListAsync();
            return clubs;
        }

        ///Returns a list of the tags for a club
        public async Task<List<Tag>> GetTags(string clubId)
        {
            List<Tag> list = await tagTable.Where(item => item.ClubId == clubId).ToListAsync();
            return list;
        }

        //NOTE: make sure that I'm delete member junction when deleting club
        ///Returns a list of the users clubs arranged by most recent activity
        public async Task<List<Club>> GetActiveClubs()
        {
            var memberList = await memberJuncTable.Where(item => item.AccountId == User.Id).ToListAsync();
            List<Club> clubList = new List<Club>();
            for (int i = 0; i < memberList.Count; i++)
            {
                string clubId = memberList[i].ClubId;
                Club club = (await clubTable.Where(item => item.Id == clubId).ToListAsync())[0];
                clubList.Add(club);
            }
            clubList.OrderByDescending(item => item.LatestActivity);
            return clubList;
        }

        ///Returns a list of DBNotifications for the user ("news") sorted by time
        public async Task<List<DBItem>> GetNewsFeed()
        {
            List<DBItem> list = new List<DBItem>();

            //by default, limits the size of each to the five most recent

            //medals
            var medalList = await medalTable.Where(item=>item.AccountId==User.Id).Take(5).ToListAsync();
            list.AddRange(medalList);
            //droplets
            var dropletList = await DBNotificationTable.Where(item => item.AccountId == User.Id && item.Type == "droplet").Take(5).ToListAsync();
            list.AddRange(dropletList);
            //rank
            list.Add(User);
            //club requests
            var clubrqList = await clubRequestTable.Where(item=>item.AccountId==User.Id).Take(5).ToListAsync();
            list.AddRange(clubrqList);
            //club invites
            var invitesList = await inviteTable.Where(item=>item.RecipientId==User.Id).Take(5).ToListAsync();
            list.AddRange(invitesList);
            //club warnings
            //NOTE: is there a better way to do this than by giving each user a warning individually?
            var warningList = await DBNotificationTable.Where(item => item.AccountId == User.Id && item.Type == "warning").Take(5).ToListAsync();
            list.AddRange(warningList);
            //friend requests
            var frqList = await friendRequestTable.Where(item => (item.AuthorId == User.Id
                || item.RecipientId == User.Id)).Take(5).ToListAsync();
            list.AddRange(frqList);

            //add clubrequests

            //TODO: THIS IS HALF DONE
            list.OrderByDescending(item=>item.Time);

            return list;
        }

        ///Create a medal that is awarded to the user; returns the name of the medal
        public async Task<string> CreateMedal(string medalName, int points) {
            //create and save medal
            Medal medal = new Medal(medalName,User.Id,points);
            await medalTable.InsertAsync(medal);

            //update the user account by adding the points from the medal to the user's rating
            User.Points += medal.Points;
            await accountTable.UpdateAsync(User);

            return medal.MedalName;
        }

        ///Get the user's medals: returns a list of medals
        public async Task<List<Medal>> GetMedals()
        {
            List<Medal> medals = await medalTable.Where(item=>item.AccountId==User.Id).ToListAsync();
            return medals;
        }

        ///Add a test DBNotification to see that they are working
        public async Task<DBNotification> TestDBNotification() {
            DBNotification DBNotification = new DBNotification(User.Id,"Medal","Test DBNotification");
            await DBNotificationTable.InsertAsync(DBNotification);
            return DBNotification;
        }

        ///Create a ban for a user; if a user accumulates 3 bans within 24 hours, they are banned for 24 hours
        public async Task<string> CreateBan(string accountId, string commentId){
            Ban ban = new Ban(accountId, commentId);
            await banTable.InsertAsync(ban);
            return ban.AccountId;
        }


        //TODO: make time added in constructors? not on server?
    }
}

