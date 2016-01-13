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

[assembly: ExportRenderer(typeof(TextCell), typeof(CloudClubv1._2_.Droid.ImprovedTextCellRenderer))]


namespace CloudClubv1._2_.Droid
{
    public class ImprovedTextCellRenderer : TextCellRenderer
    {
        protected override global::Android.Views.View GetCellCore(Cell item, global::Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var view = base.GetCellCore(item, convertView, parent, context) as ViewGroup;
            if (String.IsNullOrEmpty((item as TextCell).Text))
            {
                view.Visibility = ViewStates.Gone;
                while (view.ChildCount > 0)
                    view.RemoveViewAt(0);
                view.SetMinimumHeight(0);
                view.SetPadding(0, 0, 0, 0);
            }
            return view;
        }
    }
}