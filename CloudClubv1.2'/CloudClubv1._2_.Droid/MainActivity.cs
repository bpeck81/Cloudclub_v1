using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;

//add for push notifications
//using Gcm.Client;

namespace CloudClubv1._2_.Droid
{
    [Activity(Label = "CloudClubv1._2_", Icon = "@drawable/cloudIcon", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        static MainActivity instance;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            instance = this;

            DBWrapper dbWrapper = new DBWrapper();
            

            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(dbWrapper));

            /*
            //error handling for push notifications
            try
            {
                // Check to ensure everything's setup right
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                // Register for push notifications
                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (Java.Net.MalformedURLException)
            {
                //CreateAndShowDialog(new Exception("There was an error creating the Mobile Service. Verify the URL"), "Error");
            }
            catch (Exception e)
            {
                //CreateAndShowDialog(e, "Error");
            }*/
        }

        public static MainActivity Instance
        {
            get { return instance; }
        }
        public static MobileServiceClient MyClient
        {
            get { return DBWrapper.client; }
        }
    }
}

