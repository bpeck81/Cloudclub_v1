using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Backend;
using CloudClubv1._2_;

using Xamarin.Forms;

namespace FrontEnd
{
    public class CustomizeProfilePage : ContentPage
    {

        string chosenEmojiId;
        string chosenColorId;
        List<string> characterNames;
        List<Button> colorButtons;
        ColorHandler ch;
        TapGestureRecognizer tgr;

        public CustomizeProfilePage()
        {
            tgr = new TapGestureRecognizer();
            tgr.Tapped += Tgr_Tapped;
            ch = new ColorHandler();
            chosenEmojiId = "Dog_Character.png";
            chosenColorId = "default";
            List<Image> characterButtons = generateCharacterButtons();
            colorButtons = generateColorButtons();
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            Label headerLabel = new Label
            {
                Text = "Customize Profile",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 39
            };
            Label characterLabel = new Label
            {
                Text = "Your Emoji",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                XAlign = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))

            };

            Grid characterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto},
                    new RowDefinition { Height =  GridLength.Auto },

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },

                },
                BackgroundColor = Color.White,
                ColumnSpacing = 5,
                Padding = new Thickness(5, 5, 5, 5)
            };

            int counter = 0;
            for (int i = 0; i < characterButtons.Count / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    characterGrid.Children.Add(characterButtons[counter], i, j);
                    counter++;
                }

            }

            ScrollView characterBoxes = new ScrollView
            {
                Content = characterGrid,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Orientation = ScrollOrientation.Horizontal,
                BackgroundColor = Color.White
            };
            Label colorLabel = new Label
            {
                Text = "Your Color",
                TextColor = Color.White,
                XAlign = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };

            RowDefinition rd = new RowDefinition { Height = 60 };
            Grid colorGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = rd.Height},
                    new RowDefinition { Height =  rd.Height},

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = rd.Height },
                    new ColumnDefinition { Width = rd.Height },
                    new ColumnDefinition { Width = rd.Height},
                    new ColumnDefinition { Width = rd.Height}
                },
                HorizontalOptions = LayoutOptions.Center,
                ColumnSpacing = 20f,
                RowSpacing = 20f,
                BackgroundColor = Color.White,
                //  Padding = new Thickness(5, 20, 5, 20)
            };
            counter = 0;
            for (int i = 0; i < colorButtons.Count / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    colorGrid.Children.Add(colorButtons[counter], i, j);
                    counter++;
                }
            }


            ScrollView colorBoxes = new ScrollView
            {
                Content = colorGrid,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = ScrollOrientation.Horizontal,
                BackgroundColor = Color.White,
                Padding = new Thickness(0, 10, 10, 10)
            };

            Button bSkip = new Button
            {
                Text = "Skip",
                TextColor = Color.White,
                BackgroundColor = Color.Gray,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderRadius = 10,
                HeightRequest = 70
            };
            bSkip.Clicked += BSkip_Clicked;
            Button bContinue = new Button
            {
                Text = "Continue",
                TextColor = Color.White,
                BackgroundColor = Color.Lime,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                BorderRadius = 11,
                HeightRequest = 70,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand

            };
            bContinue.Clicked += BContinue_Clicked;
            Content = new StackLayout
            {
                Children =
                {
                    headerLabel,
                    characterLabel,
                   characterBoxes,
                    colorLabel,
                    colorBoxes,

                    new StackLayout
                    {

                        Children = {

                           bSkip,
                           bContinue
                        },
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Spacing =20,
                        Padding = new Thickness(20,10,20,20)
                    }


                },
                //     Padding = new Thickness(20, 0, 20, 20),
                Spacing = 10f,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                BackgroundColor = Color.FromRgb(210, 61, 235)
            };



        }

        private List<Button> generateColorButtons()
        {
            var colorButtons = new List<Button>();
            for (int i = 0; i < ch.colorList.Count; i++)
            {
                Button bcolor = new Button
                {

                    BackgroundColor = ch.colorList[i],
                    ClassId = BackgroundColor.ToString(),
                    BorderRadius = 5,
                    VerticalOptions = LayoutOptions.Center
                };
                bcolor.Clicked += Bcolor_Clicked;
                colorButtons.Add(bcolor);
            }

            return colorButtons;
        }



        private List<Image> generateCharacterButtons()
        {
            characterNames = new List<string>();
            characterNames.Add("Alien_Character.png");
            characterNames.Add("Baby_Character.png");
            characterNames.Add("Burger_Character.png");
            characterNames.Add("Cat_Character.png");
            characterNames.Add("Cookie_Character.png");
            characterNames.Add("Demon_Character.png");
            characterNames.Add("Devil_Character.png");
            characterNames.Add("Dog_Character.png");
            characterNames.Add("Dolphin_Character");
            characterNames.Add("Eek_Character.png");
            characterNames.Add("Ghost_Character.png");


            var imageButtons = new List<Image>();
            for (int i = 0; i < characterNames.Count; i++)
            {
                var img = new Image
                {
                    Source = FileImageSource.FromFile(characterNames[i]),
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 80,
                    WidthRequest = 80
                };
                img.GestureRecognizers.Add(tgr);
                imageButtons.Add(img);
            }
            return imageButtons;



            var characterButtons = new List<Button>();
            for (int i = 0; i < characterNames.Count(); i++)
            {

                Button bChar = new Button
                {
                    Image = characterNames[i],

                    ClassId = characterNames[i],

                    BorderRadius = 5,
                    HeightRequest = 80,
                    WidthRequest = 80,
                    VerticalOptions = LayoutOptions.Center
                };
                bChar.Clicked += BChar_Clicked;

                characterButtons.Add(bChar);
            }
            //     return characterButtons;

        }


        private void Bcolor_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                chosenColorId = ch.nameMap[b.BackgroundColor];
            }
            for (int i = 0; i < colorButtons.Count; i++)
            {
                colorButtons[i].BorderRadius = 5;
                b.BorderRadius = 150;

            }


        }
        private void BChar_Clicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                chosenEmojiId = b.Image;
            }
            System.Diagnostics.Debug.WriteLine("character Tapped!");

        }

        private void Tgr_Tapped(object sender, EventArgs e)
        {
            Image b = sender as Image;
            var source = b.Source as FileImageSource;
            if (source != null)
            {
                chosenEmojiId = source.File;
            }
            System.Diagnostics.Debug.WriteLine(chosenEmojiId);
        }


        private async void BContinue_Clicked(object sender, EventArgs e)
        {
            var clubs = new List<Club>();
            clubs = await App.dbWrapper.GetClubs();
            var popularClubs = await App.dbWrapper.GetPopularClubs();
            var newestClubs = await App.dbWrapper.GetNewestClubs();

            // send info to dbserver
            await App.dbWrapper.SetUserColor(chosenColorId);
            await App.dbWrapper.SetUserEmoji(chosenEmojiId);
            
            var navPage = new NavigationPage(new TabbedMainClubPages(clubs, new List<Club>(),popularClubs,newestClubs));
            navPage.BarBackgroundColor = ch.fromStringToColor("purple");
            Application.Current.MainPage = navPage;
        }

        private async void BSkip_Clicked(object sender, EventArgs e)
        {

            await App.dbWrapper.SetUserColor("default");
            await App.dbWrapper.SetUserEmoji("default");
            var popularClubs = await App.dbWrapper.GetPopularClubs();
            var newestClubs = await App.dbWrapper.GetNewestClubs();

            var navPage = new NavigationPage(new TabbedMainClubPages(await App.dbWrapper.GetClubs(), new List<Club>(),popularClubs,newestClubs));
            navPage.BarBackgroundColor = ch.fromStringToColor("purple");
            Application.Current.MainPage = navPage;
        }
    }
}
