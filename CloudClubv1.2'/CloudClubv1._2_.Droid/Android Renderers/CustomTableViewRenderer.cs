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
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FrontEnd.MyTableView), typeof(CloudClubv1._2_.Droid.CustomTableViewRenderer))]

namespace CloudClubv1._2_.Droid
{
    public class CustomTableViewRenderer: TableViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
                return;

            var listView = Control as global::Android.Widget.ListView;
            int[] colors = { 0,255, 255 };
            var divider = new ColorDrawable(Android.Graphics.Color.LightGray);


            listView.Divider = divider;
            listView.DividerHeight = 1;



        }



    }
}