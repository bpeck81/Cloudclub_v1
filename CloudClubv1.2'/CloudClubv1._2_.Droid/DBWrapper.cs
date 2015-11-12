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
//add for push notifications
using Gcm.Client;
//add geolocation and file pickers, etc. 
using Xamarin.Geolocation;

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
        public static IMobileServiceTable<DBNotification> dbNotificationTable = client.GetTable<DBNotification>();
        public static IMobileServiceTable<Ban> banTable = client.GetTable<Ban>();
        public static IMobileServiceTable<ClubReport> clubReportTable = client.GetTable<ClubReport>();
        public static IMobileServiceTable<TemporaryMemberJunction> temporaryMemberJuncTable = client.GetTable<TemporaryMemberJunction>();

        //used to determine how to handle push notifications
        public static string CurrentClubId = null;
        //used to determine missed updates in clubs
        public static List<string> ActiveClubs = new List<string>();

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

        /// Create an account; returns 0 if successful, 1 if username is already in use, 2 if email is
        public async Task<int> CreateAccount(string username, string password, string email)
        {
            Account account = new Account(username, password, email);

            //check if username in use
            var usernameList = await accountTable.Where(item => item.Username == account.Username).ToListAsync();
            var emailList = await accountTable.Where(item => item.Email == account.Email).ToListAsync();

            //if username in use
            if (usernameList.Count > 0 )
            {
                return 1;
            }
            //if email in use
            else if (emailList.Count > 0)
            {
                return 2;
            }
            //if ok
            else
            {
                await accountTable.InsertAsync(account);
                return 0;
            }

        }

        /// NOTE: DEPRACATED; WILL BE REMOVED SOON
        /*public async Task<bool> CreateAccount(string username, string password)
        {
            Account account = new Account(username, password, "no email");

            //check if username in use
            var usernameList = await accountTable.Where(item => item.Username == account.Username).ToListAsync();
            //var emailList = await accountTable.Where(item => item.Email == account.Email).ToListAsync();

            if (usernameList.Count > 0)
            {
                return false;
            }
            else
            {
                await accountTable.InsertAsync(account);
                return true;
            }

        }*/

        /// Login to an account; returns 1 if successful, 0 if failure
        public async Task<bool> LoginAccount(string username, string password)
        {
            var list = await accountTable.Where(item => item.Username == username && item.Password == password).ToListAsync();

            if (list.Count > 0)
            {
                //set user
                User = list[0];
                //establish push notification services
                CreatePushRegister();

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

        /// Create a club; returns true if success, false if fails; fails if club name already in use or user is banned
        public async Task<bool> CreateClub(string title, string color, bool exclusive, List<string> tagList)
        {
            Club club = new Club(title, color, exclusive, User.Id);

            //check if banned (must get current instance of user account incase ban happend since login)
            User = await accountTable.LookupAsync(User.Id);
            if (DateTime.Compare(User.Banned, DateTime.Now) > 0)
            {
                return false;
            }

            //check if the club name is in use
            var list = await clubTable.Where(item => item.Title.ToLower() == club.Title.ToLower()).ToListAsync();

            if (list.Count > 0)
            {
                return false;
            }
            else
            {
                await clubTable.InsertAsync(club);

                //add tags to table based on title
                string[] titleTagList = title.Split(' ');
                foreach (string key in titleTagList)
                {
                    Tag tag = new Tag(key.ToLower(), club.Id);
                    await tagTable.InsertAsync(tag);
                }

                //add tags to table based on tagList
                foreach (string key in tagList)
                {
                    Tag tag = new Tag(key.ToLower(), club.Id);
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

                //make DBNotification
                DBNotification DBNotification = new DBNotification(User.Id, "join", "You created the club " + club.Title + "!");
                await dbNotificationTable.InsertAsync(DBNotification);

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
            await dbNotificationTable.InsertAsync(DBNotification);

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
        /// NOTE: calling thigs with push notifications before finished registering, ERROR
        public async Task<int> CreateComment(string text, string clubId)
        {
            //check if banned (must get current instance of user account incase ban happend since login)
            User = await accountTable.LookupAsync(User.Id);
            if(DateTime.Compare(User.Banned,DateTime.Now)>0){
                return 3;
            }
            System.Diagnostics.Debug.WriteLine("mydebug---"+User.Id);

            Club club = await clubTable.LookupAsync(clubId);

            System.Diagnostics.Debug.WriteLine("mydebug----"+club.Id);

            //check if user is in club
            var list = await memberJuncTable.Where(item => item.AccountId == User.Id && item.ClubId == club.Id).ToListAsync();

            if (list.Count > 0)
            {
                Comment comment = new Comment(User.Id, clubId, text);
                await commentTable.InsertAsync(comment);

                //update user
                User.NumComments++;
                await accountTable.UpdateAsync(User);

                System.Diagnostics.Debug.WriteLine("mydebug----" + "finished");

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
        /// NOTE: calling thigs with push notifications before finished registering, ERROR
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
            await dbNotificationTable.InsertAsync(DBNotificationAuthor);

            DBNotification DBNotificationRecipient = new DBNotification(recipient.Id, "friend", "You and "+author.Username+" became friends!");
            await dbNotificationTable.InsertAsync(DBNotificationRecipient);

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
                await dbNotificationTable.InsertAsync(DBNotification);

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
            await dbNotificationTable.InsertAsync(DBNotification);

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
            await dbNotificationTable.InsertAsync(DBNotification);

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
        /// NOTE: all tags are stored in lowercase
        public async Task<List<Club>> SearchClubs(List<string> tags)
        {
            //make all tags lowercase
            for (int i = 0; i < tags.Count;i++ )
            {
                tags[i] = tags[i].ToLower();
            }

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

            //by default, limits the size of each

            //medals
            var medalList = await medalTable.OrderByDescending(item=>item.Time).Where(item=>item.AccountId==User.Id).Take(2).ToListAsync();
            list.AddRange(medalList);
            //droplets
            var dropletList = await dbNotificationTable.OrderByDescending(item => item.Time).Where(item => item.AccountId == User.Id && item.Type == "droplet").Take(4).ToListAsync();
            list.AddRange(dropletList);
            //rank
            if(User.RatingNotificationToggle){
                var rankList = await dbNotificationTable.OrderByDescending(item => item.Time).Where(item=>(item.AccountId==User.Id && item.Type=="rank")).Take(1).ToListAsync();
                list.AddRange(rankList);
            }
            //club requests
            var clubrqList = await clubRequestTable.OrderByDescending(item => item.Time).Where(item => item.AccountId == User.Id).Take(4).ToListAsync();
            list.AddRange(clubrqList);
            //club invites
            var invitesList = await inviteTable.OrderByDescending(item => item.Time).Where(item => item.RecipientId == User.Id).Take(4).ToListAsync();
            list.AddRange(invitesList);
            //club warnings
            //NOTE: is there a better way to do this than by giving each user a warning individually?
            var warningList = await dbNotificationTable.OrderByDescending(item => item.Time).Where(item => item.AccountId == User.Id && item.Type == "warning").Take(4).ToListAsync();
            list.AddRange(warningList);
            //club creations/joins
            var joinsList = await dbNotificationTable.OrderByDescending(item => item.Time).Where(item => item.AccountId == User.Id && item.Type == "join").Take(4).ToListAsync();
            list.AddRange(joinsList);
            //friend requests
            var frqList = await friendRequestTable.OrderByDescending(item => item.Time).Where(item => (item.AuthorId == User.Id
                || item.RecipientId == User.Id)).Take(4).ToListAsync();
            list.AddRange(frqList);
            //accepted friend requests
            var friendsList = await dbNotificationTable.OrderByDescending(item => item.Time).Where(item => item.AccountId == User.Id && item.Type == "friend").Take(4).ToListAsync();
            list.AddRange(friendsList);
            //bans
            var banList = await dbNotificationTable.OrderByDescending(item => item.Time).Where(item => (item.AccountId == User.Id && item.Type == "ban")).Take(2).ToListAsync();
            list.AddRange(banList);
            //club reports
            var clubReportList = await dbNotificationTable.OrderByDescending(item => item.Time).Where(item => (item.AccountId == User.Id && item.Type == "clubReport")).Take(2).ToListAsync();
            list.AddRange(clubReportList);
            

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
            User.NumMedals++;
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
            await dbNotificationTable.InsertAsync(DBNotification);
            return DBNotification;
        }

        ///Create a ban for a user; if a user accumulates 3 bans within 24 hours, they are banned for 24 hours
        ///one user can only make 1 ban; returns false if one already is made, true if a new one was made
        public async Task<bool> CreateBan(string accountId, string commentId, string reporterId){
            //check to see if the user has already made a ban
            var banList = await banTable.Where(item=> item.AccountId==accountId && item.ReporterId==reporterId).ToListAsync();

            if (banList.Count > 0)
            {
                return false;
            } else {
                Ban ban = new Ban(accountId, commentId, reporterId);
                await banTable.InsertAsync(ban);
                return true;
            }
            
        }

        ///returns the user account; returns null if not logged in
        public Account GetUser() {
            return User;
        }

        /// Creates a push register so the device can receive push notifications
        private void CreatePushRegister(){
            //error handling for push notifications
            try
            {
                // Check to ensure everything's setup right
                GcmClient.CheckDevice(MainActivity.Instance);
                GcmClient.CheckManifest(MainActivity.Instance);

                // Register for push notifications
                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmClient.Register(MainActivity.Instance, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (Java.Net.MalformedURLException)
            {
                //CreateAndShowDialog(new Exception("There was an error creating the Mobile Service. Verify the URL"), "Error");
            }
            catch (Exception e)
            {
                //CreateAndShowDialog(e, "Error");
            }
        }

        /// sets the user to null rather than an account value
        public void LogoutUser() {
            User = null;
        }

        /// returns a status int of the user and an accounts friendship: 0-not friends, 1- pending request, 2 - friends
        public async Task<int> GetFriendship(string accountId) {
            //already friends
            var friendsList = await friendsTable.Where(item => ((item.AuthorId == User.Id && item.RecipientId == accountId
                )||( item.RecipientId == User.Id && item.AuthorId == accountId))).ToListAsync();
            if(friendsList.Count>0){
                return 2;
            }
            //friend request pending
            var pendingList = await friendRequestTable.Where(item => ((item.AuthorId == User.Id && item.RecipientId == accountId
                ) || (item.RecipientId == User.Id && item.AuthorId == accountId))).ToListAsync();
            if(pendingList.Count>0){
                return 1;
            }
            //not friends in any way
            return 0;
        }

        ///returns an int value of what the user has rated the club; returns 0 if they haven't
        public async Task<int> GetUserRating(string clubId) {
            var ratingList = await ratingJunctionTable.Where(item=>(item.AccountId==User.Id && item.ClubId==clubId)).ToListAsync();
            //if they have rated it
            if (ratingList.Count > 0)
            {
                return ratingList[0].Rating;
            }
            else {
                return 0;
            }
        }

        ///Removes the user from a club; returns true if success, false if they already aren't a member
        public async Task<bool> LeaveClub(string clubId) {
            var membership = await memberJuncTable.Where(item => (item.AccountId == User.Id && item.ClubId == clubId)).ToListAsync();
            if(membership.Count>0){
                await memberJuncTable.DeleteAsync(membership[0]);
                return true;
            }else{
                return false;
            }
        }

        ///Returns a list of accounts that are the user's friends
        public async Task<List<Account>> GetFriends(string accountId) {
            var friendsJunc = await friendsTable.Where(item=>(item.AuthorId==accountId || item.RecipientId==accountId)).ToListAsync();
            List<Account> friends = new List<Account>();
            for (int i = 0; i < friendsJunc.Count;i++ )
            {
                Account friend;
                //loop through and make sure to add the friend, not the user, to the friendlist
                if(friendsJunc[i].AuthorId==User.Id){
                    friend = await GetAccount(friendsJunc[i].RecipientId);
                }else{
                    friend = await GetAccount(friendsJunc[i].AuthorId);
                }
                
                friends.Add(friend);
            }
            return friends;
        }

        ///Returns the number of friends the user and a given account share
        ///NOTE: not 100% sure this works, but I think it does
        public async Task<int> GetSharedFriendsCount(string accountId) {
            var userFriends = await GetFriends(User.Id);
            var accountFriends = await GetFriends(accountId);
            int sharedFriends=0;
            //loop throuh user's frieds
            for (int i = 0; i < userFriends.Count;i++ )
            {
                for (int j = 0; j < accountFriends.Count;j++ )
                {
                    //if they share a friend
                    if(accountFriends[j].Id==userFriends[i].Id){
                        sharedFriends++;
                        break;
                    }
                }
            }
            return sharedFriends;
        }

        ///returns true if the user and a given account share any of the same clubs; false if not
        public async Task<bool> InSameClub(string accountId) {
            var userMemberships = await memberJuncTable.Where(item=>item.AccountId==User.Id).ToListAsync();
            var accountMemberhsips = await memberJuncTable.Where(item => item.AccountId == accountId).ToListAsync();

            for (int i = 0; i < userMemberships.Count;i++ )
            {
                for (int j = 0; j < accountMemberhsips.Count;j++ )
                {
                    //if they share club
                    if(accountMemberhsips[j].ClubId==userMemberships[i].ClubId){
                        return true;
                    }
                }
            }
            return false;

        }

        ///returns true if the user has a pending club request for a given club, false if not
        public async Task<bool> IsPendingClubRequest(string clubId) {
            var clubRequests = await clubRequestTable.Where(item=>(item.AccountId==User.Id && item.ClubId==clubId)).ToListAsync();
            if(clubRequests.Count>0){
                return true;
            }else{
                return false;
            }
        }

        ///returns true if the there is a pending invite to a club for an account, false if not
        public async Task<bool> IsPendingInvite(string clubId,string accountId)
        {
            var invites = await inviteTable.Where(item=>(item.ClubId==clubId && item.RecipientId==accountId)).ToListAsync();
            if (invites.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        ///return true if the given account is a memer of the club, false if not
        ///NOTE: this differs from is member in that ismember assumes the account is the user, isclubmember does not
        public async Task<bool> IsClubMember(string clubId, string accountId) {
            var members = await memberJuncTable.Where(item=>(item.ClubId==clubId && item.AccountId==accountId)).ToListAsync();
            if(members.Count>0){
                return true;
            }else{
                return false;
            }
        }

        ///returns true if a given account has rated a comment, false if not
        public async Task<bool> HasRatedComment(string accountId, string commentId) {
            var ratings = await commentJuncTable.Where(item=>(item.CommentId==commentId && item.RaterId==accountId)).ToListAsync();
            if(ratings.Count>0){
                return true;
            }else{
                return false;
            }
        }

        ///Delete a given tag for a club; returns true if success, false if failure
        public async Task<bool> DeleteTag(string tag,string clubId) {
            var tagList = await tagTable.Where(item => (item.Key == tag.ToLower() && item.ClubId == clubId)).ToListAsync();
            if (tagList.Count > 0)
            {
                await tagTable.DeleteAsync(tagList[0]);
                return true;
            }
            else {
                return false;
            }
        }

        ///returns the timespan from the most recent comment in a club to now
        public async Task<TimeSpan> TimeSinceActivity(string clubId)
        {
            var comments = await commentTable.Where(item => item.ClubId == clubId).OrderByDescending(item => item.Time).Take(1).ToListAsync();
            return (TimeSpan)(DateTime.Now - comments[0].Time);
        }

        //NOTE: these functions are unecessary since the things is static, but for consistency sake, here they are
        ///must be called when viewing a new club; notifys backend of what club is currenty being viewed for 
        ///push notification purposes
        public async Task SetCurrentClubId(string clubId) {
            CurrentClubId = clubId;

            //create a temporary member junction to enable chat updating if not in club
            if(!(await IsMember(clubId))){
                var tempMember = new TemporaryMemberJunction(User.Id,clubId);
                await temporaryMemberJuncTable.InsertAsync(tempMember);
            }

            //remove from active clubs now that viewing it
            if(ActiveClubs.Contains(clubId)){
                ActiveClubs.Remove(clubId);
            }
        }

        ///returns the id of the club the user is currently looking at
        public string GetCurrentClubId() {
            return CurrentClubId;
        }

        ///must be called when leaving view of club;sets the id of the current club the user is looking at to null
        public async Task RemoveCurrentClubId()
        {
            //delete a temporary membership if not a member
            var tempMemberships = (await temporaryMemberJuncTable.Where(item => item.ClubId == CurrentClubId && item.AccountId == User.Id)
                .Take(1).ToListAsync());
            if(tempMemberships.Count>0){
                await temporaryMemberJuncTable.DeleteAsync(tempMemberships[0]);
            }

            CurrentClubId = null;
        }

        ///turn on push notification ranking updates for the user
        public async Task<bool> EnableRankingNotification() {
            User.RatingNotificationToggle = true;
            await accountTable.UpdateAsync(User);
            return User.RatingNotificationToggle;
        }

        ///turn off push notification ranking updates for the user
        public async Task<bool> DisableRankingNotification()
        {
            User.RatingNotificationToggle = false;
            await accountTable.UpdateAsync(User);
            return User.RatingNotificationToggle;
        }

        ///returns a list of accounts that are members of a given club
        public async Task<List<Account>> GetClubMembers(string clubId) {
            var memberJuncs = await memberJuncTable.Where(item=>item.ClubId==clubId).ToListAsync();
            List<Account> memberAccounts = new List<Account>();
            for (int i = 0; i < memberJuncs.Count;i++ )
            {
                memberAccounts.Add(await accountTable.LookupAsync(memberJuncs[i].AccountId));
            }
            return memberAccounts;
        }

        ///returns a list of clubs a given account is member of
        public async Task<List<Club>> GetAccountClubs(string accountId)
        {
            var clubMemberships = await memberJuncTable.Where(item=>item.AccountId==accountId).ToListAsync();
            List<Club> accountsClubs = new List<Club>();
            for (int i = 0; i < clubMemberships.Count;i++ )
            {
                accountsClubs.Add(await clubTable.LookupAsync(clubMemberships[i].ClubId));
            }
            return accountsClubs;
        }

        ///returns a list of comments and club requests; is what users see in chat
        ///returns 20-25 items; index is used to get sets of 20, ex: 0 is most recent 20, 1 is next most recent 20...
        public async Task<List<DBItem>> GetChat(string clubId, int index) {
            List<DBItem> list = new List<DBItem>();

            //get 20 most recent comments
            var commentList = await commentTable.OrderByDescending(item => item.Time).Where(item=> item.ClubId==clubId)
                .Skip(index*20).Take(20).ToListAsync();
            //get 5 most recent club requests
            var requestList = await clubRequestTable.OrderByDescending(item => item.Time).Where(item => item.ClubId == clubId)
                .Skip(index*5).Take(5).ToListAsync();
            //add to list and sort
            list.AddRange(commentList);
            list.AddRange(requestList);
            list.OrderByDescending(item=>item.Time);

            return list;
        }

        ///Create a report for a club; if a club accumulates 6 reports within 24 hours, it is deleted;
        ///returns if report was made, fails if user already made a report
        public async Task<bool> CreateClubReport(string clubId, string reporterId)
        {
            //check if the user has already made a club report
            var clubReportList = await clubReportTable.Where(item=> item.ClubId==clubId && item.ReporterId==reporterId).ToListAsync();

            //get club title
            var club = await clubTable.LookupAsync(clubId);

            //if report already exists
            if(clubReportList.Count>0){
                return false;
            //if it doesn't already exist
            }else{
                ClubReport clubReport = new ClubReport(clubId, reporterId, club.Title);
                await clubReportTable.InsertAsync(clubReport);
                return true;
            }
            
        }

        ///Returns the location a user is in; returns college name if in registered college, none if not in valid college
        public async Task<string> GetLocation() {
            string location = "none";

            //college and latitude,longitude
            List<string> colleges = new List<string>();
            List<double[]> collegeLocations = new List<double[]>();
            //radius of 2 miles; unit is degrees
            double radius = .0290;
            //add colleges
            colleges.Add("UVA");
            collegeLocations.Add(new double[]{38.0350,-78.5050});
            

            System.Diagnostics.Debug.WriteLine("mydebug--started getting position");

            var locator = new Geolocator(MainActivity.Instance) { DesiredAccuracy = 50 };
            await locator.GetPositionAsync(timeout: 10000).ContinueWith(t =>
            {
                System.Diagnostics.Debug.WriteLine("mydebug--Position Status: {0}", t.Result.Timestamp);
                System.Diagnostics.Debug.WriteLine("mydebug--Position Latitude: {0}", t.Result.Latitude);
                System.Diagnostics.Debug.WriteLine("mydebug--Position Longitude: {0}", t.Result.Longitude);

                //loop through colleges and find if in location
                for (int i = 0; i < colleges.Count;i++ )
                {
                    double latDist = Math.Abs(t.Result.Latitude-collegeLocations[i][0]);
                    double lonDist = Math.Abs(t.Result.Longitude-collegeLocations[i][1]);
                    if(latDist<radius && lonDist<radius){
                        location = colleges[i];
                        break;
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            return location;
        }

        ///Returns a club given an id
        public async Task<Club> GetClub(string clubId)
        {
            var club = await clubTable.LookupAsync(clubId);
            return club;
        }


        //TODO: make time added in constructors? not on server?
    }
}

