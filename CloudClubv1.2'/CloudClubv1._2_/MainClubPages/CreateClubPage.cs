using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using CloudClubv1._2_;
using Backend;
using Xamarin.Forms;

namespace FrontEnd
{
    public class CreateClubPage : ContentPage
    {
        bool clubPublic;
        string clubColor;
        ScrollView colorBoxes;
        Label titleLabel, colorLabel, tagsLabel, inviteLabel, lcouldntCreate;
        Grid colorGrid;
        Button bContinue, bAddFriends, bPublic, bPrivate;
        public Button bAddTags; //Must fix
        
        List<string> tagList;       
        List<Button> colorButtons;
        Entry clubNameEntry;
        public AddTagsPage tagPage;
        ColorHandler ch;
        public CreateClubPage()

        {
            ch = new ColorHandler();
            tagPage = new AddTagsPage();
            clubPublic = true;
            clubColor = "default";
            tagList = new List<string>();
            colorButtons = this.generateColorButtons();
            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);
            this.Title = "Create A Club";
            titleLabel = new Label
            {
                Text = "Title",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 42,
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center
            };
            clubNameEntry = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.White,
                TextColor = Color.Black,
                Placeholder =  "Club Name"
            };
             colorLabel = new Label
            {
                Text = "Color",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 42,
                XAlign = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center
            };
            RowDefinition rd = new RowDefinition { Height = 60 };

             colorGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Center,

                RowDefinitions =
                {
                    new RowDefinition { Height = rd.Height},
                    new RowDefinition { Height =  rd.Height},

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = rd.Height },
                    new ColumnDefinition { Width = rd.Height },
                    new ColumnDefinition { Width = rd.Height },
                    new ColumnDefinition { Width = rd.Height }

                },
                
               HorizontalOptions = LayoutOptions.CenterAndExpand,
                RowSpacing = 5,
                ColumnSpacing = 10,
                BackgroundColor = Color.White,
                Padding = new Thickness(70, 5, 70,5)
            };
            int counter = 0;
            for (int i = 0; i < (colorButtons.Count) / 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    colorGrid.Children.Add(colorButtons[counter], i, j);
                    counter++;
                }
            }


             inviteLabel = new Label
            {
                Text = "Invite Friends",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center

            };
             tagsLabel = new Label
            {
                Text = "Add Tags",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center

            };
            bAddTags = new Button
            {
                Text = tagPage.addedTags.Count.ToString(),
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("lightGray"),
                FontAttributes =FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderRadius = 7,
                VerticalOptions = LayoutOptions.Center
            };
            bAddTags.Clicked += BAddTags_Clicked;
            bAddFriends = new Button
            {
                Text = "+",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = Color.FromRgb(210,61,235),
                BackgroundColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderRadius = 7,
                VerticalOptions = LayoutOptions.Center
            };
            bAddFriends.Clicked += BAddFriends_Clicked;

             bPublic = new Button
            {
                Text = "Public",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("lightGrayPressed"),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderRadius = 7,
                VerticalOptions = LayoutOptions.Center
            };
            bPublic.Clicked += BPublic_Clicked;
             bPrivate = new Button
            {
                Text = "Private",
                TextColor = Color.White,
                BackgroundColor = ch.fromStringToColor("red"),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderRadius = 7,
                VerticalOptions = LayoutOptions.Center
            };
            bPrivate.Clicked += BPrivate_Clicked;
             bContinue = new Button
            {
                Text = "Continue",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.Lime,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 80,
                BorderRadius = 15
            };
            bContinue.Clicked += BContinue_Clicked;

            lcouldntCreate = new Label
            {
                Text = "coudllnt cjrer",
                TextColor = Color.Red,
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center

            };


            Content = updatePage();


        }

        private async void BContinue_Clicked(object sender, EventArgs e)
        {
            //NOTE: for the parameter for exclusive, true means it is exclusive, but whether or not the club is public is tracked here, so reverse it when creating club
            bool created = await App.dbWrapper.CreateClub(this.clubNameEntry.Text, clubColor,!clubPublic,tagList);
            if (created)
            {
                await Navigation.PopAsync();
            }
            else
            {
                lcouldntCreate.Text = "Club Name Already In Use";
                updatePage();
            }
        }
        private StackLayout updatePage()
        {

           StackLayout sLayout = new StackLayout
            {

                Children =
                        {

                            titleLabel,
                    //        lcouldntCreate,
                            clubNameEntry,
                            colorLabel,
                            colorGrid,
                            new StackLayout
                            {
                                Children =
                                {
                                    inviteLabel,
                                    tagsLabel,
                                },
                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                                Spacing = 70,
                                Padding = new Thickness(5,5,5,5),
                                VerticalOptions = LayoutOptions.Center

                             },
                             new StackLayout
                             {
                                 Children =
                                 {
                                     bAddFriends,
                                     bAddTags
                                  },

                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.Center,
                                Spacing = 10,
                                Padding  = new Thickness(5,5,5,5)
                             },
                             new StackLayout
                             {
                                 Children =
                                 {
                                     bPrivate,
                                     bPublic
                                 },

                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.Center,
                                Spacing = 10,
                                Padding = new Thickness(5,5,5,5)
                             },
                             bContinue

                        },
                BackgroundColor = Color.FromRgb(210, 61, 235),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand

            };

               
                return sLayout;
           
        }

        private void BAddFriends_Clicked(object sender, EventArgs e)
        {

        }

        private void BAddTags_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(tagPage);
            var frontTagList = tagPage.addedTags;
            tagList = new List<string>();
            for(int i = 0; i< frontTagList.Count; i++)
            {
                tagList.Add(frontTagList[i].Tag);
            }
            bAddTags.Text = tagList.Count.ToString();
         }

        private void BPrivate_Clicked(object sender, EventArgs e)
        {
            clubPublic = false;
            bPrivate.BackgroundColor = ch.fromStringToColor("redPressed");
            bPublic.BackgroundColor = ch.fromStringToColor("lightGrayPressed");
        }

        private void BPublic_Clicked(object sender, EventArgs e)
        {
            clubPublic = true;
            bPrivate.BackgroundColor = ch.fromStringToColor("red");
            bPublic.BackgroundColor = ch.fromStringToColor("lightGray");

        }

        private List<Button> generateColorButtons()
        {
            List<Button> colorButtons = new List<Button>();
            
            for (int i = 0; i < ch.colorList.Count; i++)
            {
                Button bcolor = new Button
                {

                    BackgroundColor = ch.colorList[i],
                    BorderRadius = 5,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                bcolor.Clicked += Bcolor_Clicked; 
                colorButtons.Add( bcolor);
            }

            return colorButtons;
        }

        private void Bcolor_Clicked(object sender, EventArgs e)
        {
            for(int i  = 0; i<colorButtons.Count; i++)
            {
                colorButtons[i].BorderRadius = 5;
            }
            Button b = sender as Button;
            b.BorderRadius = 200;
            ColorHandler ch = new ColorHandler();
            clubColor = ch.nameMap[b.BackgroundColor];

        }
    }
}
