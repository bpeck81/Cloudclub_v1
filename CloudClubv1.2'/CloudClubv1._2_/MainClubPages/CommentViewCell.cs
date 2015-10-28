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
        TapGestureRecognizer dropletTGR;
        int dropletPressedCount;
       public CommentViewCell()
        {
            dropletPressedCount = 0;
            dropletTGR = new TapGestureRecognizer();
            dropletTGR.Tapped += DropletTGR_Tapped;
            ch = new ColorHandler();
            Image userEmoji = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Scale = 1,
                HeightRequest =30

            };
            userEmoji.SetBinding(Image.SourceProperty, "UserEmoji");
            Label lUserId = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center

            };
            lUserId.SetBinding(Label.TextProperty, "AuthorUsername");
            lUserId.SetBinding(Label.TextColorProperty, "AuthorAccountColor", converter: new ColorConverter());
            Label lDropletNumber = new Label
            {
                TextColor = ch.fromStringToColor("black"),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center

            };
            lDropletNumber.SetBinding(Label.TextProperty, "NumDroplets");

            Image dropletImage = new Image
            {
                Source = FileImageSource.FromFile("DropletFull_WhiteB.png"),
                Aspect = Aspect.AspectFit,
                HeightRequest = 25,        
                      
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center

            };
            dropletImage.GestureRecognizers.Add(dropletTGR);
            
            Label lCommentText = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = ch.fromStringToColor("black"),
                
                
            };
            lCommentText.SetBinding(Label.TextProperty, "Text");
            System.Diagnostics.Debug.WriteLine("Comment Text " +lCommentText.Text);
           
            //lCommentText.SetBinding(Label.TextColorProperty, "TextColor", converter: new ColorConverter());
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
                Spacing = 11,
                VerticalOptions = LayoutOptions. Center,
                HorizontalOptions = LayoutOptions.Fill
            };

            View = new StackLayout
            {
                Children =
               {
                   headerLayout,
                   new StackLayout
                   {
                       Children =
                       {
                           lCommentText
                       },
                       Padding = new Thickness(5,0,0,0)
                       
                   }
                },
                BackgroundColor = ch.fromStringToColor("white"),
                Spacing =11,
                Padding = new Thickness(12,10,10,10),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
          
            };
//System.Diagnostics.Debug.WriteLine("DropletsClicked "+ dropletNumber.ToString());
        }

        private async void DropletTGR_Tapped(object sender, EventArgs e)
        {
            dropletPressedCount++;
            var thisComment = (FrontComment)BindingContext;

            await App.dbWrapper.RateComment(thisComment.Id);

        }
        private int getCustomCellHeight(string commentText)
        {
            double heightRequest = 30;
            // assume a height of 10:1 height to line ratio where one line has 50 chars
            heightRequest = commentText.Length/5;


            return (int) heightRequest;
        }
    }
}
