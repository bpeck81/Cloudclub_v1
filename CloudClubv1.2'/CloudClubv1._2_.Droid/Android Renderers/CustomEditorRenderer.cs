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
using FrontEnd;

[assembly:ExportRenderer(typeof(MyEditor), typeof(CloudClubv1._2_.Droid.CustomEditorRenderer))]
namespace CloudClubv1._2_.Droid
{
    public class CustomEditorRenderer :EditorRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                Control.SetTextColor(Android.Graphics.Color.Black);
            }
        }

    }
}