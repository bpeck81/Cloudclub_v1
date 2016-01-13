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
using Backend;
using Xamarin.Forms;
using Android.Graphics;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(CustomDailyRankSwitch), typeof(CloudClubv1._2_.Droid.CustomDRSwitchCellRenderer))]
namespace CloudClubv1._2_.Droid
{
    class CustomDRSwitchCellRenderer :SwitchCellRenderer
    {
        
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var cell = base.GetCellCore(item, convertView, parent, context);

           
            var swtchCell = cell as SwitchCellView;


            if (swtchCell != null)
            {
                 var swtch = swtchCell.AccessoryView as global::Android.Widget.Switch;

                if (swtch != null)
                {
                    swtch.SetTextColor(Android.Graphics.Color.Black);
                    
                    swtch.TextAlignment = Android.Views.TextAlignment.TextStart;
                    
                    swtch.Text = "Daily Ranking Notification                                         ";//TODO: Find a Perminent Fix

                }
            }
            return cell;
        }
    }
}