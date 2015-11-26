using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using PCLStorage;

using Xamarin.Forms;
using Backend;
using CloudClubv1._2_;


namespace FrontEnd
{
    public class AlternateCustomizeProfilePage : ContentPage
    {
        string chosenEmojiId;
        string chosenColorId;
        List<string> characterNames;
        List<Button> colorButtons;
        List<Image> characterButtons;
        ColorHandler ch;
        TapGestureRecognizer tgr, characterBoxTgr, extendedImagesTgr;
        Label headerLabel, characterLabel, colorLabel;
        Grid characterGrid, colorGrid;
        ScrollView characterBoxes, colorBoxes;
        Button bSkip, bContinue;
        Image displayedEmoji;
        public AlternateCustomizeProfilePage()

        {
            characterBoxTgr = new TapGestureRecognizer();
            extendedImagesTgr = new TapGestureRecognizer();
            extendedImagesTgr.Tapped += ExtendedImagesTgr_Tapped;
            characterBoxTgr.Tapped += CharacterBoxTgr_Tapped;
            ch = new ColorHandler();
            chosenEmojiId = "Dog_Character.png";
            chosenColorId = "purple";
            characterButtons = generateExtendedCharacterButtons();
            displayedEmoji = characterButtons[0];
            displayedEmoji.GestureRecognizers.Add(characterBoxTgr);

            colorButtons = generateColorButtons();
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);



            this.generateInitialView();
        }



        private void generateInitialView()
        {
            headerLabel = new Label
            {
                Text = "Customize Profile",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White,
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 39
            };
            characterLabel = new Label
            {
                Text = "Tap the box below to choose your Emoji",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                XAlign = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))

            };

            characterGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,

                RowDefinitions =
                {
                    new RowDefinition { Height = 80},

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = 80 },


                },
                BackgroundColor = Color.White,
                ColumnSpacing = 5,

                Padding = new Thickness(5, 5, 5, 5)
            };
            characterGrid.GestureRecognizers.Add(characterBoxTgr);

            characterGrid.Children.Add(displayedEmoji, 0, 0);

            colorLabel = new Label
            {
                Text = "Your Color",
                TextColor = Color.White,
                XAlign = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };

            RowDefinition rd = new RowDefinition { Height = 60 };
            colorGrid = new Grid
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

            bSkip = new Button
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
            bContinue = new Button
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
                    characterGrid,
                   //characterBoxes,
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


        private void generateExtendedCharacterView()
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
            Content = new StackLayout
            {
                Children =
                {
                    headerLabel,
                    characterScrollView,
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
                Spacing = 10f,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                BackgroundColor = Color.FromRgb(210, 61, 235)
            };

        }
        private void CharacterBoxTgr_Tapped(object sender, EventArgs e)
        {
            generateExtendedCharacterView();
        }

        private List<Image> generateExtendedCharacterButtons()
        {

            var characterNames = new List<string>();
            characterNames.Add("AF1.png");
            characterNames.Add("AF33.png");
            characterNames.Add("AF32.png");
            characterNames.Add("AF30.png");
            characterNames.Add("AF38.png");
            characterNames.Add("AF3.png");
            characterNames.Add("AF40.png");
            characterNames.Add("AF15.png");
            characterNames.Add("AN11.png");
            characterNames.Add("AN13.png");
            characterNames.Add("AN20.png");
            characterNames.Add("AN19.png");
            characterNames.Add("AN31.png");
            characterNames.Add("AN28.png");
            characterNames.Add("AN33.png");
            characterNames.Add("AN5.png");
            characterNames.Add("AA10.png");
            characterNames.Add("AA11.png");
            characterNames.Add("AA12.png");
            characterNames.Add("AA13.png");
            characterNames.Add("AA31.png");
            characterNames.Add("AA30.png");
            characterNames.Add("AA1.png");
            characterNames.Add("AA17.png");
            characterNames.Add("AA17.png");
            characterNames.Add("AA17.png");
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
                var img = new Image
                {
                    Source = FileImageSource.FromFile(characterNames[i]),
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 70,
                    WidthRequest = 70
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
            characterNames.Add("AF1.png");


            var imageButtons = new List<Image>();
            for (int i = 0; i < characterNames.Count; i++)
            {
                var img = new Image
                {
                    Source = FileImageSource.FromFile(characterNames[i]),
                    Aspect = Aspect.AspectFit,
                    HeightRequest = 60,
                    WidthRequest = 60
                };

            }
            return imageButtons;


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



        private void ExtendedImagesTgr_Tapped(object sender, EventArgs e)
        {
            Image b = sender as Image;
            var source = b.Source as FileImageSource;
            if (source != null)
            {
                chosenEmojiId = source.File;
            }
            displayedEmoji.Source = b.Source;

            generateInitialView();

            System.Diagnostics.Debug.WriteLine(chosenEmojiId);

        }

        private async void BContinue_Clicked(object sender, EventArgs e)
        {
            var clubs = await App.dbWrapper.GetClubs();
            var popularClubs = await App.dbWrapper.GetPopularClubs();
            var newestClubs = await App.dbWrapper.GetNewestClubs();

            // send info to dbserver
            await App.dbWrapper.SetUserColor(this.chosenColorId);
            await App.dbWrapper.SetUserEmoji(this.chosenEmojiId);
            var memberClubsList = await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id);
            var pendingClubList = new List<string>();
            //TODO check if get clubs returns all clubs
            for (int i = 0; i < clubs.Count; i++)
            {
                if (await App.dbWrapper.IsPendingClubRequest(clubs[i].Id))
                {
                    pendingClubList.Add(clubs[i].Id);
                }

            }
            List<string> firstLineCommentList = new List<string>();
            for (int i = 0; i < memberClubsList.Count; i++)
            {
                var comment = await App.dbWrapper.GetRecentComment(memberClubsList[i].Id);

                firstLineCommentList.Add(comment.Text);
            }

            var navPage = new NavigationPage(new TabbedMainClubPages(clubs, memberClubsList, popularClubs, newestClubs, pendingClubList, firstLineCommentList));
            navPage.BarBackgroundColor = ch.fromStringToColor("purple");
            navPage.BackgroundColor = ch.fromStringToColor("purple");
            Application.Current.MainPage = navPage;
        }

        private async void BSkip_Clicked(object sender, EventArgs e)
        {

            await App.dbWrapper.SetUserColor(chosenColorId);
            await App.dbWrapper.SetUserEmoji(chosenEmojiId);
            var popularClubs = await App.dbWrapper.GetPopularClubs();
            var newestClubs = await App.dbWrapper.GetNewestClubs();
            var clubs = await App.dbWrapper.GetClubs();
            var memberClubsList = await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id);
            List<string> firstLineCommentList = await App.getMostRecentComment(memberClubsList);

            List<string> pendingInviteList = new List<string>();

            for (int i = 0; i < clubs.Count; i++)
            {
                if (await App.dbWrapper.IsPendingClubRequest(clubs[i].Id))
                {
                    pendingInviteList.Add(clubs[i].Id);
                }
            }
            var navPage = new NavigationPage(new TabbedMainClubPages(clubs, memberClubsList, popularClubs, newestClubs, pendingInviteList, firstLineCommentList));
            navPage.BarBackgroundColor = ch.fromStringToColor("purple");
            Application.Current.MainPage = navPage;
        }
    }
}



