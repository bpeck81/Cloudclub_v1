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
    public class ChatInfoPage : ContentPage
    {
        ColorHandler ch;
        public ChatInfoPage(List<Tag> tagsList, FrontClub club)
        {
            ch = new ColorHandler();
            Title = "Info";
            BackgroundColor = ch.fromStringToColor("white");
            Button bTags = new Button
            {
                Text = "Tags",
                TextColor = ch.fromStringToColor("red"),
                BackgroundColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            Button bRating = new Button
            {
                Text = "Rating",
                TextColor = ch.fromStringToColor("gray"),
                BackgroundColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            bRating.Clicked += BRating_Clicked;
            StackLayout topBarSLayout = new StackLayout
            {
                Children =
                {
                    bTags,
                    bRating,
                },
                Orientation = StackOrientation.Horizontal
                
            };
            var tagsListView = new ListView
            {
                ItemsSource = tagsList,
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Label
                    {
                        BackgroundColor = ch.fromStringToColor("gray"),
                        TextColor = ch.fromStringToColor("black")
                    };
                    label.SetBinding(Label.TextProperty, "Key");
                    return new ViewCell
                    {
                        View = label
                    };
                }
                )

            };

            var lMember = new Label
            {
                Text = "Members",
                TextColor = ch.fromStringToColor("red"),
                FontAttributes = FontAttributes.Bold,
                XAlign = TextAlignment.Center
            };

            Button bSubscribe = new Button
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Text = "Subscribe",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = ch.fromStringToColor("green"),
                BorderRadius = 15,
                HeightRequest = 40

            };
            bSubscribe.Clicked += async (sender, e) =>
            {
                //TODO: see what first paramater is in createclubrequest
                await App.dbWrapper.CreateClubRequest(" ", club.Id);
                bSubscribe.IsVisible = false;
            };
            Content = new StackLayout
            {
                Children =
                {
                    topBarSLayout,
                    tagsListView,
                    lMember,
                    bSubscribe
                }
            };
        }

        private void BRating_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
