﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Backend;
using CloudClubv1._2_;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrontEnd
{


    public class SignUpPage : ContentPage
    {
        string username, password, invalidSignupText;
        Entry userNameEntry, passwordEntry;
        ColorHandler ch;
        public SignUpPage()
        {
            // sign up and Login toggle page 
            ch = new ColorHandler();
            invalidSignupText = "";
            this.displaySignUpContent();

        }


        private void displaySignUpContent()
        {
            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);
            Label invalidSignupLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.Red,
                HorizontalOptions = LayoutOptions.Center

            };
            // sign up page
            Label topHeader = new Label
            {
                Text = "Sign Up",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold,
                //    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                FontSize = 42,
                TextColor = Color.White,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                BackgroundColor = Color.FromRgb(210, 61, 235)

            };
            this.userNameEntry = new Entry
            {
                Placeholder = "Username",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.Black,
               

                BackgroundColor = Color.White,

            };
            this.passwordEntry = new Entry
            {
                Placeholder = "Password",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsPassword = true,
                TextColor = Color.Black,
                BackgroundColor = Color.White

            };


            Button continueB = new Button
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BorderWidth = 1,
                Text = "Continue",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.Lime,
                TextColor = Color.White,
                HeightRequest = 110,
                WidthRequest =335,
                BorderRadius = 20


            };
            continueB.Clicked += ContinueSignUpB_Clicked;

            Label orLabel = new Label
            {
                Text = "Or",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center

            };

            Button loginB = new Button
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Text = "Login",
                BorderWidth = 10,
                HeightRequest = 110,
                WidthRequest = 335,
                FontAttributes = FontAttributes.Bold,
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BackgroundColor = Color.Gray,
                TextColor = Color.White,
                BorderRadius = 20

            };
            loginB.Clicked += LoginB_Clicked;


            StackLayout entryFields = new StackLayout
            {
                Children =
                {
                    userNameEntry,
                    passwordEntry

                },
                VerticalOptions = LayoutOptions.Center,
                
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 1f
            };

            StackLayout sLayout = new StackLayout
            {
                Children = {
                    topHeader,
                  //  invalidSignupLabel,
                    entryFields,
                    new StackLayout
                    {
                        Children =
                        {
                            continueB,
                         //   orLabel,
                            loginB,
                        },
                        Spacing = 20f,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand

                    }
                 },
                BackgroundColor = Color.FromRgb(210, 61, 235),
                Spacing = 50f,
                Padding = new Thickness(0,0,0,20),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            Content = sLayout;



        }

        private void displayLoginContent()
        {
            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);

            // sign login page
            Label topHeader = new Label
            {
                Text = "Login",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold,
                //    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                FontSize = 42,
                TextColor = Color.White,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                BackgroundColor = Color.FromRgb(210, 61, 235)

            };
            Label invalLoginLabel = new Label
            {
                Text = "Invalid Login",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.Red,
                HorizontalOptions = LayoutOptions.Center

            };
            userNameEntry = new Entry
            {
                Placeholder = "Username",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.Black,

                BackgroundColor = Color.White,

            };
            passwordEntry = new Entry
            {
                Placeholder = "Password",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsPassword = true,
                TextColor = Color.Black,
                BackgroundColor = Color.White

            };

            Button continueB = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BorderWidth = 1,
                Text = "Continue",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.Lime,
                TextColor = Color.White,
                HeightRequest = 110,
                WidthRequest = 335,
                BorderRadius = 20


            };
            continueB.Clicked += ContinueLoginB_Clicked;

            Label orLabel = new Label
            {
                Text = "Or",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center

            };

            Button signUpB = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Text = "Sign Up",
                BorderWidth = 10,
                HeightRequest = 110,
                WidthRequest = 335,
                FontAttributes = FontAttributes.Bold,
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BackgroundColor = Color.Blue,
                TextColor = Color.White,
                BorderRadius = 20

            };
            signUpB.Clicked += BSignUp_Clicked;


            StackLayout entryFields = new StackLayout
            {
                Children =
                {
                    userNameEntry,
                    passwordEntry

                },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 1f
            };

            StackLayout sLayout = new StackLayout
            {
                Children = {
                    topHeader,
                    entryFields,
                    new StackLayout
                    {
                        Children =
                        {
                            continueB,
                           // orLabel,
                            signUpB,
                        },
                        Spacing = 20f,
                        Padding = new Thickness(25,0,25,0),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand

                    }
                 },
                BackgroundColor = Color.FromRgb(210, 61, 235),
                Spacing = 50f,
                Padding = new Thickness(0, 0, 0, 20),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            Content = sLayout;

        }

        private async void ContinueLoginB_Clicked(object sender, EventArgs e)
        {
            username = userNameEntry.Text;
            password = userNameEntry.Text;

            if (await App.dbWrapper.LoginAccount(username, password))
            {
                List<Club> clubList = await App.dbWrapper.GetClubs();
                List<Club> memberClubList = new List<Club>();
                var popularClubs = await App.dbWrapper.GetPopularClubs();
                var newestClubs = await App.dbWrapper.GetNewestClubs();
                for (int i =0; i<clubList.Count; i++)
                {
                    if(await App.dbWrapper.IsMember(clubList[i].Id))
                    {
                        memberClubList.Add(clubList[i]);
                    }
                }
            //    var userAccount = await App.dbWrapper.
                var navPage = new NavigationPage(new TabbedMainClubPages(clubList, memberClubList, popularClubs,newestClubs));
                navPage.BarBackgroundColor = ch.fromStringToColor("purple");
                Application.Current.MainPage = navPage;

            }/// return value if dne check 
        }

        private void BSignUp_Clicked(object sender, EventArgs e)
        {
            this.displaySignUpContent();
        }

        private async void ContinueSignUpB_Clicked(object sender, EventArgs e)
        {
            this.username = userNameEntry.Text;
            this.password = passwordEntry.Text;
            string validityText = "valid";//await checkUserPassValidity(username, password);
            if (validityText.Equals("valid"))
            {
                await App.dbWrapper.CreateAccount(username, password);
                await App.dbWrapper.LoginAccount(username, password);
                await Navigation.PushModalAsync(new CustomizeProfilePage());

            }
            else
            {
                invalidSignupText = validityText;
                this.displaySignUpContent();
            }


        }

        private void LoginB_Clicked(object sender, EventArgs e)
        {
            
            this.displayLoginContent();


        }
        private async Task<string> checkUserPassValidity(string username, string password)
        {
            string validityCase = "Invalid Entry";
            string[] invalidChars = new string[13] { " ", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", ".", "," };
            bool usernameValid = false, passwordValid = false;
            bool login = await App.dbWrapper.LoginAccount(username, password); 
           if (username != null)
            {
                if (login == true) 
                {
                    return "Username Exists";
                }
                for (int i = 0; i < invalidChars.Length; i++)
                {
                    if (username.Contains(invalidChars[i]))
                    {
                        return "Username Contains Invalid Characters";
                    }
                }
                if (username.Length < 5)
                {
                    return "Username Too Short";
                }
                usernameValid = true;

            }
            if (password != null)
            {
                if (password.Length < 7)
                {
                    return "Password Must Be Greater Than 6 Characters";
                }
                else
                {
                    passwordValid = true;
                }

            }
            if(usernameValid && passwordValid)
            {
                return "valid";

            }
            return validityCase;

        }
    }
}