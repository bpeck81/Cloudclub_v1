using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CloudClubv1._2_;

namespace FrontEnd
{
    class ChangeEmojiPage: ContentPage
    {
        List<string> characterNames;
        List<Image> characterButtons;
        List<Button> colorButtons;
        ColorHandler ch;
        string chosenColorId;
        string chosenEmojiId;
        TapGestureRecognizer extendedImagesTgr, backButtonTgr;
        Grid characterGrid, colorGrid;
        ScrollView colorBoxes;
        Image bBack;


        public ChangeEmojiPage(string currentEmoji, string currentColor)
        {
            ch = new ColorHandler();
            chosenColorId = currentColor;
            chosenEmojiId = currentEmoji;
            characterNames = new List<string>();
            NavigationPage.SetHasNavigationBar(this, false);

            characterButtons = this.generateExtendedCharacterButtons();
            colorButtons = this.generateColorButtons();

            extendedImagesTgr = new TapGestureRecognizer();
            extendedImagesTgr.Tapped += ExtendedImagesTgr_Tapped;
            backButtonTgr = new TapGestureRecognizer();
            backButtonTgr.Tapped += BackButtonTgr_Tapped;

            Content = new StackLayout
            {
                Children =
                {
                    generateActionBar(),
                    generateColorView(),
                   generateExtendedCharacterView()

                },
                
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ch.fromStringToColor("white")
            };
            


        }


        private View generateActionBar()
        {
            bBack = new Image
            {
                Source = FileImageSource.FromFile("arrow_back.png"),
                WidthRequest = 30,
                // Scale = ,

                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 30,
                Aspect = Aspect.AspectFill
            };
            bBack.GestureRecognizers.Add(backButtonTgr);

            var actionBarLabel = new Label
            {
               Text = "Customize Your Profile",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = ch.fromStringToColor("white"),
                FontSize = 22,
                FontAttributes = FontAttributes.Bold
            };

            var actionBarLayout = new StackLayout
            {
                Children =
                {
                    
                    bBack,
                    actionBarLabel

                },
                HeightRequest = 30,
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = ch.fromStringToColor("purple"),
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(10, 10, 0, 10)
            };
            return actionBarLayout;
        }
        private async void BackButtonTgr_Tapped(object sender, EventArgs e)
        {

            await App.dbWrapper.SetUserColor(this.chosenColorId);
            await App.dbWrapper.SetUserEmoji(this.chosenEmojiId);

            MessagingCenter.Send<ChangeEmojiPage>(this, "updateData");
            await Navigation.PopAsync();
        }

        private void ExtendedImagesTgr_Tapped(object sender, EventArgs e)
        {

            Image b = sender as Image;
            var source = b.Source as FileImageSource;
            foreach(Image character  in characterButtons)
            {
                var s = character.Source as FileImageSource;
                if(chosenEmojiId == s.File)
                {
                    character.Scale = 1;
                }
            }
            if (source != null)
            {
                chosenEmojiId = source.File;
            }

            foreach (Image character in characterButtons)
            {
                
                if (character.Source == b.Source)
                {
                    character.Scale = .7;
                    //character.Source = FileImageSource.FromFile("cloudIcon.png");
                }
            }
        }

        private ScrollView generateColorView()
        {
            RowDefinition rd = new RowDefinition { Height = 60,};
            colorGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                RowDefinitions =
                {
                    new RowDefinition { Height = 50},
                    new RowDefinition { Height =  50},

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = 50 },
                    new ColumnDefinition { Width = 50 },
                    new ColumnDefinition { Width = 50},
                    new ColumnDefinition { Width = 50}
                },
                HorizontalOptions = LayoutOptions.Center,
                
                ColumnSpacing = 20f,
                RowSpacing = 20f,
                BackgroundColor = Color.White,
                //  Padding = new Thickness(5, 20, 5, 20)
            };
            int counter = 0;
            for (int i = 0; i < colorButtons.Count / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    colorGrid.Children.Add(colorButtons[counter], i, j);
                    counter++;
                }
            }


            colorBoxes = new ScrollView
            {
                Content = colorGrid,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Orientation = ScrollOrientation.Horizontal,
                BackgroundColor = Color.White,
                Padding = new Thickness(0, 10, 10, 10)
            };
            return colorBoxes;
        }
        private View generateExtendedCharacterView()
        {
            // characterButtons = generateExtendedCharacterButtons();

            characterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto},
                    new RowDefinition { Height =  GridLength.Auto },
                    new RowDefinition { Height =  GridLength.Auto },
                    new RowDefinition { Height =  GridLength.Auto },
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
            for (int i = 0; i < characterButtons.Count / 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    characterButtons[counter].GestureRecognizers.Add(extendedImagesTgr);
                    characterGrid.Children.Add(characterButtons[counter], i, j);
                    counter++;
                }
            }
            ScrollView characterScrollView = new ScrollView
            {
                Content = characterGrid,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ch.fromStringToColor("white"),
                Orientation = ScrollOrientation.Horizontal
            };
            return characterScrollView;
        }

        private List<Image> generateExtendedCharacterButtons()
        {

            //     var characterNames = new List<string>();
            characterButtons = new List<Image>();

            characterNames.Add("AF1.png");
            characterNames.Add("AF2.png");
            characterNames.Add("AF3.png");
            characterNames.Add("AF4.png");
            characterNames.Add("AF5.png");
            characterNames.Add("AF6.png");
            characterNames.Add("AF7.png");
            characterNames.Add("AF8.png");
            characterNames.Add("AF9.png");
            characterNames.Add("AF10.png");
            characterNames.Add("AF11.png");
            characterNames.Add("AF12.png");
            characterNames.Add("AF13.png");
            characterNames.Add("AF14.png");
            characterNames.Add("AF15.png");
            characterNames.Add("AF16.png");
            characterNames.Add("AF17.png");
            characterNames.Add("AF18.png");
            characterNames.Add("AF19.png");
            characterNames.Add("AF20.png");
            characterNames.Add("AF21.png");
            characterNames.Add("AF22.png");
            characterNames.Add("AF23.png");
            characterNames.Add("AF24.png");
            characterNames.Add("AF25.png");
            characterNames.Add("AF26.png");
            characterNames.Add("AF27.png");
            characterNames.Add("AF28.png");
            characterNames.Add("AF29.png");
            characterNames.Add("AF30.png");
            characterNames.Add("AF31.png");
            characterNames.Add("AF32.png");
            characterNames.Add("AF33.png");
            characterNames.Add("AF34.png");
            characterNames.Add("AF35.png");
            characterNames.Add("AF36.png");
            characterNames.Add("AF37.png");
            characterNames.Add("AF38.png");
            characterNames.Add("AF39.png");
            characterNames.Add("AF40.png");
            characterNames.Add("Nature");
            characterNames.Add("AN1.png");
            characterNames.Add("AN2.png");
            characterNames.Add("AN3.png");
            characterNames.Add("AN4.png");
            characterNames.Add("AN5.png");
            characterNames.Add("AN6.png");
            characterNames.Add("AN7.png");
            characterNames.Add("AN8.png");
            characterNames.Add("AN9.png");
            characterNames.Add("AN10.png");
            characterNames.Add("AN11.png");
            characterNames.Add("AN12.png");
            characterNames.Add("AN13.png");
            characterNames.Add("AN14.png");
            characterNames.Add("AN15.png");
            characterNames.Add("AN16.png");
            characterNames.Add("AN17.png");
            characterNames.Add("AN18.png");
            characterNames.Add("AN19.png");
            characterNames.Add("AN20.png");
            characterNames.Add("AN21.png");
            characterNames.Add("AN22.png");
            characterNames.Add("AN23.png");
            characterNames.Add("AN24.png");
            characterNames.Add("AN25.png");
            characterNames.Add("AN26.png");
            characterNames.Add("AN27.png");
            characterNames.Add("AN28.png");
            characterNames.Add("AN29.png");
            characterNames.Add("AN30.png");
            characterNames.Add("AN31.png");
            characterNames.Add("AN32.png");
            characterNames.Add("AN33.png");
            characterNames.Add("AN34.png");
            characterNames.Add("AN35.png");
            characterNames.Add("AN36.png");
            characterNames.Add("AN37.png");
            characterNames.Add("AN38.png");
            characterNames.Add("AN39.png");
            characterNames.Add("AN40.png");
            characterNames.Add("Interests");
            characterNames.Add("AA1.png");
            characterNames.Add("AA2.png");
            characterNames.Add("AA3.png");
            characterNames.Add("AA4.png");
            characterNames.Add("AA5.png");
            characterNames.Add("AA6.png");
            characterNames.Add("AA7.png");
            characterNames.Add("AA8.png");
            characterNames.Add("AA9.png");
            characterNames.Add("AA10.png");
            characterNames.Add("AA11.png");
            characterNames.Add("AA12.png");
            characterNames.Add("AA13.png");
            characterNames.Add("AA14.png");
            characterNames.Add("AA15.png");
            characterNames.Add("AA16.png");
            characterNames.Add("AA17.png");
            characterNames.Add("AA18.png");
            characterNames.Add("AA19.png");
            characterNames.Add("AA20.png");
            characterNames.Add("AA21.png");
            characterNames.Add("AA22.png");
            characterNames.Add("AA23.png");
            characterNames.Add("AA24.png");
            characterNames.Add("AA25.png");
            characterNames.Add("AA26.png");
            characterNames.Add("AA27.png");
            characterNames.Add("AA28.png");
            characterNames.Add("AA29.png");
            characterNames.Add("AA30.png");
            characterNames.Add("AA31.png");
            characterNames.Add("AA32.png");
            characterNames.Add("AA33.png");
            characterNames.Add("AA34.png");
            characterNames.Add("AA35.png");
            characterNames.Add("AA36.png");
            characterNames.Add("AA37.png");
            characterNames.Add("AA38.png");
            characterNames.Add("AA39.png");
            characterNames.Add("AA40.png");


            var imageButtons = new List<Image>();
            for (int i = 0; i < characterNames.Count; i++)
            {
                double scale = 1;
                if(chosenEmojiId == characterNames[i])
                {
                    scale = .7;
                }
                var img = new Image
                {
                    Source = FileImageSource.FromFile(characterNames[i]),
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 70,
                    WidthRequest = 70,
                    Scale  = scale
                };
                img.GestureRecognizers.Add(extendedImagesTgr);
                imageButtons.Add(img);
            }
            return imageButtons;


        }

        private List<Button> generateColorButtons()
        {
            var colorButtons = new List<Button>();
            for (int i = 0; i < ch.colorList.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine(this.chosenColorId);

                var radius = 5;
                if (chosenColorId != null && chosenColorId != "")
                {


                    if (ch.fromStringToColor(this.chosenColorId) == ch.colorList[i])
                    {
                        radius = 150;
                    }
                }

                Button bcolor = new Button
                {

                    BackgroundColor = ch.colorList[i],
                    ClassId = BackgroundColor.ToString(),
                    BorderRadius = radius,
                    VerticalOptions = LayoutOptions.Center
                };
                bcolor.Clicked += Bcolor_Clicked;
                colorButtons.Add(bcolor);
            }

            return colorButtons;
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

    }
}
