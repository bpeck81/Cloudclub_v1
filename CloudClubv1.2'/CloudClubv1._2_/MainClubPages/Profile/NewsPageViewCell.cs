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
                VerticalOptions = LayoutOptions.Center

            };
            imgNotification.SetBinding(Image.SourceProperty, "NotificationImage");
            Label lNotificaton = new Label
            {
                TextColor = ch.fromStringToColor("black"),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            lNotificaton.SetBinding(Label.TextProperty, "Text");
            Label lTimeSpan = new Label
            {
                TextColor = ch.fromStringToColor("black"),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
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
                Padding = new Thickness(7, 0, 7, 0)
            };
            View = sLayout;
        }

        
    }
}
