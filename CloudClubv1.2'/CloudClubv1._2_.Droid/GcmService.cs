//lol, copy and pasted 100%

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
using Android.Util;

using Gcm.Client;
using Microsoft.WindowsAzure.MobileServices;
using FrontEnd;
using CloudClubv1._2_;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]


namespace CloudClubv1._2_.Droid
{
    [Service]
    public class GcmService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }

        public GcmService()
            : base(PushHandlerBroadcastReceiver.SENDER_IDS) { }

        //needed to add this
        protected override void OnUnRegistered(Context context, string registrationId)
        {
            throw new NotImplementedException();
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            RegistrationID = registrationId;

            MobileServiceClient client = MainActivity.MyClient;

            var push = client.GetPush();

            //pass the user's id as a tag
            List<string> tags = new List<string>();
            tags.Add(App.dbWrapper.GetUser().Id);

            MainActivity.Instance.RunOnUiThread(() => Register(push, tags));

        }
        public async void Register(Microsoft.WindowsAzure.MobileServices.Push push, IEnumerable<string> tags)
        {
            try
            {
                //NOTE: apparently the data must be called message
                const string template = "{\"data\":{\"message\":\"$(message)\"}}";

                await push.RegisterTemplateAsync(RegistrationID, template, "mytemplate", tags);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        protected async override void OnMessage(Context context, Intent intent)
        {
            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
            }

            //Store the message
            /*
            var prefs = GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            var edit = prefs.Edit();
            edit.PutString("last_msg", msg.ToString());
            edit.Commit();*/

            
            string message = intent.Extras.GetString("message");
            //message has format: "type of notification,id of class associate w/ notification"
            string[] parsedMessage = message.Split(',');

            if (!string.IsNullOrEmpty(message))
            {
                if(parsedMessage[0].Equals("comment")){

                    var comment = await DBWrapper.commentTable.LookupAsync(parsedMessage[1]);
                    var account = await DBWrapper.accountTable.LookupAsync(comment.AuthorId);
                    var club = await DBWrapper.clubTable.LookupAsync(comment.ClubId);

                    //ClubChatPage.CurrentCommentsList.Add(new FrontComment(comment, account));

                    createNotification("Cloudclub", "People have been talking in "+club.Title+".");
                    return;
                }
                else if (parsedMessage[0].Equals("like"))
                {

                    var dbComment = await DBWrapper.commentTable.LookupAsync(parsedMessage[1]);

                    //hacky solution; i should implement the interface inotifypropertychanged https://msdn.microsoft.com/en-us/library/vstudio/ms743695(v=vs.100).aspx
                    var comment = ClubChatPage.CurrentCommentsList.FirstOrDefault(item=>item.Id==dbComment.Id);
                    comment.NumDroplets = dbComment.NumDroplets;
                    int index = ClubChatPage.CurrentCommentsList.IndexOf(comment);
                    ClubChatPage.CurrentCommentsList.Remove(comment);
                    ClubChatPage.CurrentCommentsList.Insert(index,comment);


                    System.Diagnostics.Debug.WriteLine("COMMENT LIKED");

                    return;
                }
            }

        }

        void createNotification(string title, string desc)
        {
            //Create notification
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show ui
            var uiIntent = new Intent(this, typeof(MainActivity));

            //Create the notification
            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

            //Auto cancel will remove the notification once the user touches it
            notification.Flags = NotificationFlags.AutoCancel;

            //Set the notification info
            //we use the pending intent, passing our ui intent over which will get called
            //when the notification is tapped.
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

            //Show the notification
            notificationManager.Notify(1, notification);
        }
        protected override void OnError(Context context, string errorId)
        {
            Log.Error(PushHandlerBroadcastReceiver.TAG, "GCM Error: " + errorId);
        }
    }

    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]

    public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string TAG = "I put this in?";
        public static string[] SENDER_IDS = new string[] { "716620987480" };

    }
}