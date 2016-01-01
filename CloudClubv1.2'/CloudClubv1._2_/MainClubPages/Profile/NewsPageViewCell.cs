using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using Backend;
using CloudClubv1._2_;
namespace FrontEnd
{
    public class NewsPageViewCell : ViewCell
    {
        ColorHandler ch;
        public NewsPageViewCell()
        {
            ch = new ColorHandler();
            Image imgNotification = new Image
            {
                Aspect = Aspect.AspectFit,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Scale =.7

            };
            imgNotification.SetBinding(Image.SourceProperty, "NotificationImage");
            Label lNotificaton = new Label
            {
                TextColor = ch.fromStringToColor("black"),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                LineBreakMode  = LineBreakMode.CharacterWrap
            };
            lNotificaton.SetBinding(Label.TextProperty, "Text");
            Label lTimeSpan = new Label
            {
                TextColor = ch.fromStringToColor("black"),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
                LineBreakMode   = LineBreakMode.NoWrap
            };
            lTimeSpan.SetBinding(Label.TextProperty, "Time");

            var sLayout = new StackLayout
            {
                Children =
                {
                    imgNotification,
                    lNotificaton,
                    
                    lTimeSpan
                },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(2, 0, 7, 0)
            };
            View = sLayout;
        }


    }
}
