using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CloudClubv1._2_.Droid
{
    [Activity(Label = "CloudClubv1._2_", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            DBWrapper dbWrapper = new DBWrapper();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(dbWrapper));
        }
    }
}

