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
using Android.Text;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(MySearchBar), typeof(CloudClubv1._2_.Droid.CustomSearchBarRenderer))]

namespace CloudClubv1._2_.Droid
{
    class CustomSearchBarRenderer :SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            base.OnElementChanged(args);

            SearchView searchView = (base.Control as SearchView);
            searchView.SetInputType(InputTypes.ClassText | InputTypes.TextVariationNormal);

            int textViewId = searchView.Context.Resources.GetIdentifier("android:id/search_src_text", null, null);
            EditText textView = (searchView.FindViewById(textViewId) as EditText);

            textView.SetBackgroundColor(Android.Graphics.Color.White);
            textView.SetTextColor(Android.Graphics.Color.Black);
            textView.SetHintTextColor(Android.Graphics.Color.Gray);

            int frameId = searchView.Context.Resources.GetIdentifier("android:id/search_plate", null, null);
            Android.Views.View frameView = (searchView.FindViewById(frameId) as Android.Views.View);
            frameView.SetBackgroundColor(Android.Graphics.Color.Gray);

        }

    }
}