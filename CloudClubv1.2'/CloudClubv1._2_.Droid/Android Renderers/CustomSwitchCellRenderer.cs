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
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CloudClubv1._2_.Droid.CustomSwitchCellRenderer))]
namespace CloudClubv1._2_.Droid
{
    class CustomSwitchCellRenderer : SwitchCellRenderer
    {
        
        SwitchCellView swtchCell;
        
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var cell = base.GetCellCore(item, convertView, parent, context);
            CustomSwitch cs = (CustomSwitch)item;
            swtchCell = cell as SwitchCellView;

            
            if (swtchCell != null)
            {
                var swtch = swtchCell.AccessoryView as global::Android.Widget.Switch;

                if (swtch != null)
                {
                    swtch.SetTextColor(Android.Graphics.Color.Black);

                    swtch.TextAlignment = Android.Views.TextAlignment.TextStart;
                    swtch.Text = cs.Text+ "                                                                              "; //TODO: Find fix for spacing

                }
            }
            
            return cell;
        }

       

    }

}