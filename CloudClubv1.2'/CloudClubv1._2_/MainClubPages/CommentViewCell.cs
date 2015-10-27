using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using CloudClubv1._2_;
using Xamarin.Forms;

namespace FrontEnd
{
    class CommentViewCell: ViewCell
    {

        ColorHandler ch;
       public CommentViewCell()
        {
            ch = new ColorHandler();
            Image userEmoji = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center

            };
            userEmoji.SetBinding(Image.SourceProperty, "UserEmoji");
            Label lUserId = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center

            };
            lUserId.SetBinding(Label.TextProperty, "AuthorId");
            lUserId.SetBinding(Label.TextColorProperty, "AuthorAccountColor", converter: new ColorConverter());
            Label lDropletNumber = new Label
            {
                TextColor = ch.fromStringToColor("white"),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center

            };
            lDropletNumber.SetBinding(Label.TextProperty, "NumDroplets");
            Image dropletImage = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center

            };

            Label lCommentText = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
            lCommentText.SetBinding(Label.TextProperty, "Text");
            lCommentText.SetBinding(Label.TextColorProperty, "TextColor", converter: new ColorConverter());
            StackLayout headerLayout = new StackLayout
            {
                Children =
                {
                    userEmoji,
                    lUserId,
                    lDropletNumber,
                    dropletImage
                },
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions. Center,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            View = new StackLayout
            {
                Children =
               {
                   headerLayout,
                   lCommentText,
                },
                BackgroundColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
          
            };
        }
    }
}
