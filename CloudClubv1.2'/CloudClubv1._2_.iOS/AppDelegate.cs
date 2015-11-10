using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Foundation;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;

namespace CloudClubv1._2_.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
		static UIApplication instance;

		private ApnPushes APushes;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			DBWrapper dbWrapper = new DBWrapper ();
			instance = app;
			APushes = new ApnPushes ();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App(dbWrapper));

            return base.FinishedLaunching(app, options);
        }

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			// Modify device token
			string _deviceToken = deviceToken.Description;
			_deviceToken = _deviceToken.Trim('<', '>').Replace(" ", "");

			// Get Mobile Services client
			MobileServiceClient client = DBWrapper.client;

			// Register for push with Mobile Services
			IEnumerable<string> tag = new List<string>() { App.dbWrapper.GetUser().Id };

			const string template = "{\"aps\":{\"alert\":\"$(message)\"}}";

			var expiryDate = DateTime.Now.AddDays(90).ToString
				(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));

			var push = client.GetPush();

			push.RegisterTemplateAsync (_deviceToken, template, expiryDate, "myTemplate", tag);
		}

		public async override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			NSObject inAppMessage;

			bool success = userInfo.TryGetValue(new NSString("inAppMessage"), out inAppMessage);

			if (success)
			{
				string[] parsedMessage = inAppMessage.ToString ().Split (',');

				//used for updating chats
				if(parsedMessage[0].Equals("comment")){
					await APushes.CommentPush(parsedMessage);
				}
				else if (parsedMessage[0].Equals("like"))
				{
					await APushes.LikePush(parsedMessage);
				}
				//standard use for notifications
				else if(parsedMessage[0].Equals("medal")){
					APushes.MedalPush(parsedMessage);
				}
				else if (parsedMessage[0].Equals("droplet"))
				{
					await APushes.DropletPush(parsedMessage);
				}
				else if (parsedMessage[0].Equals("dbNotification"))
				{
					APushes.DBNotificationPush(parsedMessage);
				}
				else if (parsedMessage[0].Equals("clubRequest"))
				{
					await APushes.ClubRequestPush(parsedMessage);
				}

			}
		}
			

    }
}
