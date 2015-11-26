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
    class ContactUsPage : ContentPage
    {
        ColorHandler ch;
        Editor Editor;

        public ContactUsPage()
        {
            ch = new ColorHandler();
            Title = "Contact Us";

            Editor = new MyEditor
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Fill,
                HeightRequest = 200,
                
                //  Placeholder = "Message",
                //  TextColor = ch.fromStringToColor("black"),
                BackgroundColor = ch.fromStringToColor("white")
            };
            Button bCancel = new Button
            {
                Text = "X",
                TextColor = ch.fromStringToColor("white"),
                BackgroundColor = ch.fromStringToColor("lightGray"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 100
            };
            bCancel.Clicked += BCancel_Clicked;
            Button bSend = new Button
            {
                Text = "Send",
                TextColor = ch.fromStringToColor("white"),
                BackgroundColor = ch.fromStringToColor("green"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 100
            };
            bSend.Clicked += BSend_Clicked;

            Content = new StackLayout
            {
                Children =
                {
                    Editor,
                    new StackLayout
                    {
                        Children =
                        {
                            bCancel,
                            bSend
                        },
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Spacing =10,
                        Padding = new Thickness(10,10,10,10)
                    }
                },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0, 0, 0, 70),
                BackgroundColor = ch.fromStringToColor("purple")
            };
        }

        private void BCancel_Clicked(object sender, EventArgs e)
        {
            Editor.Text = "";

        }

        private async void BSend_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Alert", "Your message has been sent", "OK");
            Navigation.PopToRootAsync();
            //send message to db
        }
    }
}
