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
using Xamarin.Forms.Platform.Android;
using FrontEnd;
using Xamarin.Forms;
using Android.Graphics;
using Backend;
using CloudClubv1._2_.Droid;
using Android.Graphics.Drawables;
using Android.Util;

[assembly: ExportRenderer(typeof(MyTabbedPage), typeof(CustomTabRenderer))]

namespace CloudClubv1._2_.Droid
{

    public class CustomTabRenderer :TabbedRenderer
    {
        private Activity activity;
        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            activity = this.Context as Activity;
            if (e.OldElement != null || Element == null)
            {
                return;
            }

        }

        protected override void DispatchDraw(global:: Android.Graphics.Canvas canvas)
        {

            ActionBar actionBar = activity.ActionBar;
            ColorDrawable colorDrawable = new ColorDrawable(Android.Graphics.Color.Rgb(210,65,235));
            actionBar.SetStackedBackgroundDrawable(colorDrawable);
            

            if (actionBar.TabCount >0){
                
                actionBarImageSetup(actionBar);

            }
            base.DispatchDraw(canvas);

        }
        private void actionBarImageSetup(ActionBar actionBar)
        {

           
           var clubSearch = actionBar.GetTabAt(0);
            ImageView csTabImage = new ImageView(activity);
            clubSearch.SetIcon(Resource.Drawable.search_Android50);

            var myClubs = actionBar.GetTabAt(1);
            var mcTabImage = new ImageView(activity);
            myClubs.SetIcon(Resource.Drawable.club_Android50);

            var profile = actionBar.GetTabAt(2);
            var pTabImage = new ImageView(activity);
            profile.SetIcon(Resource.Drawable.profile_Android50);

        }
    }
}