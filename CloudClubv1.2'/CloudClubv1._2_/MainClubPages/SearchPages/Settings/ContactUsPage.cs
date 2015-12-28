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
        Editor editor;

        public ContactUsPage()
        {
            ch = new ColorHandler();
            Title = "Contact Us";

            editor = new MyEditor
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Fill,
                HeightRequest = 200,
            
                BackgroundColor = ch.fromStringToColor("white")
            };
            Button bCancel = new Button
            {
                Text = "X",
                TextColor = ch.fromStringToColor("white"),
                BackgroundColor = ch.fromStringToColor("lightGray"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontAttributes = FontAttributes.Bold,
               // VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize= 32,
                BorderRadius =10,
                WidthRequest = 20,
                HeightRequest = 100
            };
            bCancel.Clicked += BCancel_Clicked;
            Button bSend = new Button
            {
                Text = "Send",
                TextColor = ch.fromStringToColor("white"),
                FontSize = 32,
                BackgroundColor = ch.fromStringToColor("green"),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BorderRadius =10,
                
                HeightRequest = 100
            };
            bSend.Clicked += BSend_Clicked;

            Content = new StackLayout
            {
                Children =
                {
                    editor,
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
            editor.Text = "";

        }

        private async void BSend_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Alert", "Your message has been sent", "OK");
            Navigation.PopToRootAsync();
            await App.dbWrapper.CreateContactUs(editor.Text);
        }
    }
}
