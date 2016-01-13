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
using CloudClubv1._2_.Droid;

namespace CloudClubv1._2_.Droid
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory =true,Label = "SplashActivity")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}