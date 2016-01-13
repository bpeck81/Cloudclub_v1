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
using Xamarin.Forms;
using Android.Graphics.Drawables;
using FrontEnd;

[assembly: ExportRenderer(typeof(MyViewCell), typeof(CloudClubv1._2_.Droid.CustomClubViewCellRenderer))]

namespace CloudClubv1._2_.Droid
{
    public class CustomClubViewCellRenderer:ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
          
            var view = base.GetCellCore(item, convertView, parent, context) as Android.Views.View;
            var gd = new GradientDrawable();
            gd.SetCornerRadius(65);
            gd.SetColor( Android.Graphics.Color.White);
            view.SetBackgroundDrawable(gd);
            return view;
        }

    }
}