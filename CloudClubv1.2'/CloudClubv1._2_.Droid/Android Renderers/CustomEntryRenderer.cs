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
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Backend;
using FrontEnd;
[assembly:ExportRenderer(typeof(MyEntry), typeof(CloudClubv1._2_.Droid.CustomEntryRenderer))]
namespace CloudClubv1._2_.Droid
{
    class CustomEntryRenderer:EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                Control.SetHintTextColor(Android.Graphics.Color.Gray);
                
                
            }
        }
    }
}