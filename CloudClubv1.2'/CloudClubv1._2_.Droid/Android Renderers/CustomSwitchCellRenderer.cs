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

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CloudClubv1._2_.Droid.CustomSwitchCellRenderer))]
namespace CloudClubv1._2_.Droid
{
    class CustomSwitchCellRenderer :SwitchCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var cell = base.GetCellCore(item, convertView, parent, context);
            cell.SetBackgroundColor(Android.Graphics.Color.Rgb(210,65,235));
            //cell.TextAlignment = Android.Views.TextAlignment.TextEnd;        
            return cell;
        }
    }
}