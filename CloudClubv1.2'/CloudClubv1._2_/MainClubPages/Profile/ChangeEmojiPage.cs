using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
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
        TapGestureRecognizer extendedImagesTgr;
        Grid characterGrid, colorGrid;
        ScrollView colorBoxes;


        public ChangeEmojiPage()
        {
            ch = new ColorHandler();
            characterButtons = this.generateExtendedCharacterButtons();
            characterNames = new List<string>();
            colorButtons = this.generateColorButtons();
            chosenColorId = "default";
            chosenEmojiId = "default";
            extendedImagesTgr = new TapGestureRecognizer();
            extendedImagesTgr.Tapped += ExtendedImagesTgr_Tapped;

            Content = new StackLayout
            {
                Children =
                {
                   // generateColorView(),
                   generateExtendedCharacterView()

                },
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ch.fromStringToColor("white")
            };
            


        }

        private void ExtendedImagesTgr_Tapped(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private ScrollView generateColorView()
        {
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
            return colorBoxes;
        }
        private Grid generateExtendedCharacterView()
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
            return characterGrid;
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
