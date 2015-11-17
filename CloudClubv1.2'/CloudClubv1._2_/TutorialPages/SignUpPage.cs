﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Backend;
using CloudClubv1._2_;
using System.Threading.Tasks;
using Xamarin.Forms;
using PCLStorage;

namespace FrontEnd
{


    public class SignUpPage : ContentPage
    {
        string username, password, invalidSignupText;
        Entry userNameEntry, passwordEntry, emailEntry;
        ColorHandler ch;
        Label invalidSignupLabel, invalidLoginLabel;
        bool invalidSignup, invalidLogin;
        public SignUpPage()
        {
            // sign up and Login toggle page 
            ch = new ColorHandler();
            invalidSignupText = "";
            this.displaySignUpContent();
            invalidLogin = false;
            invalidSignup = false;

        }


        private void displaySignUpContent()
        {
            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);
            invalidSignupLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = ch.fromStringToColor("white"),
                Text = "Invalid Entry",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                IsVisible = invalidSignup

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
            this.emailEntry = new Entry
            {
                Placeholder = "Email",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = ch.fromStringToColor("black"),
                BackgroundColor = Color.White
            };



            Button continueB = new Button
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BorderWidth = 1,
                Text = "Continue",
                //     Font = Font.SystemFontOfSize(NamedSize.Large),
                FontSize = 36,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.Lime,
                TextColor = Color.White,
                HeightRequest = 110,
                WidthRequest = 335,
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
                FontSize = 36,
                WidthRequest = 335,
                FontAttributes = FontAttributes.Bold,
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
                    passwordEntry,
                    emailEntry

                },
                VerticalOptions = LayoutOptions.Center,

                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 1f
            };

            StackLayout sLayout = new StackLayout
            {
                Children = {
                    topHeader,
                    invalidSignupLabel,
                    entryFields,
                    new StackLayout
                    {
                        Children =
                        {
                            continueB,
                            loginB,
                        },
                        Spacing = 20f,
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

        private void displayLoginContent()
        {
            this.Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0);

            // sign login page
            Label invalidLoginText = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 24,
                TextColor = ch.fromStringToColor("white"),
                FontAttributes = FontAttributes.Bold
            };
            Label topHeader = new Label
            {
                Text = "Login",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = 42,
                TextColor = Color.White,
                FontFamily = Device.OnPlatform(iOS: "MarkerFelt-Thin", Android: "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                BackgroundColor = Color.FromRgb(210, 61, 235)

            };
            invalidLoginLabel = new Label
            {
                Text = "Invalid Login",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = ch.fromStringToColor("white"),
                HorizontalOptions = LayoutOptions.Center,
                IsVisible = invalidLogin

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
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.Lime,
                TextColor = Color.White,
                FontSize = 36,
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
                FontSize = 36,
                WidthRequest = 335,
                FontAttributes = FontAttributes.Bold,
                //Font = Font.SystemFontOfSize(NamedSize.Large),
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
                    invalidLoginLabel,
                    entryFields,
                    new StackLayout
                    {
                        Children =
                        {
                            continueB,
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
                invalidLogin = false;
                var fileSystem = FileSystem.Current.LocalStorage;
                var exists = await fileSystem.CheckExistsAsync("PhoneData.txt");
                var userId = App.dbWrapper.GetUser().Id;

                if (exists.Equals(ExistenceCheckResult.FileExists))
                {
                    IFile file = await fileSystem.GetFileAsync("PhoneData.txt");
                    var fileCopy = await file.ReadAllTextAsync();
                    var fileLines = fileCopy.Split('\n');
                    var idLine = fileLines[0].Split(':');
                    idLine[1] = userId.ToString();
                    string lineString = idLine[0] + ':' + idLine[1];
                    fileLines[0] = lineString;
                    string contents = "";
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        contents += fileLines[i] + '\n';
                    }
                    System.Diagnostics.Debug.WriteLine(contents);
                    await file.WriteAllTextAsync(contents);
                }



                List<Club> clubs = await App.dbWrapper.GetClubs();
                var popularClubs = await App.dbWrapper.GetPopularClubs();
                var newestClubs = await App.dbWrapper.GetNewestClubs();
                var memberClubList = await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id);

                //    var userAccount = await App.dbWrapper.
                var memberClubsList = await App.dbWrapper.GetAccountClubs(App.dbWrapper.GetUser().Id);

                List<string> pendingInviteList = new List<string>();

                for (int i = 0; i < clubs.Count; i++)
                {
                    if (await App.dbWrapper.IsPendingClubRequest(clubs[i].Id))
                    {
                        pendingInviteList.Add(clubs[i].Id);
                    }
                }
                var navPage = new NavigationPage(new TabbedMainClubPages(clubs, memberClubsList, popularClubs, newestClubs, pendingInviteList));
                navPage.BarBackgroundColor = ch.fromStringToColor("purple");
                Application.Current.MainPage = navPage;

            }/// return value if dne check 
            else
            {
                invalidLoginLabel.Text = "Invaild Username or Password";
                invalidLogin = true;
                displayLoginContent();
            }
        }

        private void BSignUp_Clicked(object sender, EventArgs e)
        {
            this.displaySignUpContent();
        }

        private async void ContinueSignUpB_Clicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;

            this.username = userNameEntry.Text;
            this.password = passwordEntry.Text;


            bool emailValidity = checkEmailValidity(email);
            string validityText = "valid";//await checkUserPassValidity(username, password);
            if (validityText.Equals("valid") && emailValidity)
            {
                invalidSignup = false;
                System.Diagnostics.Debug.WriteLine(email);
                await App.dbWrapper.CreateAccount(username, password, email);
                await App.dbWrapper.LoginAccount(username, password);
                var userId = App.dbWrapper.GetUser().Id;


                var fileSystem = FileSystem.Current.LocalStorage;
                var exists = await fileSystem.CheckExistsAsync("PhoneData.txt");

                if (exists.Equals(ExistenceCheckResult.FileExists))
                {
                    IFile file = await fileSystem.GetFileAsync("PhoneData.txt");
                    var fileCopy = await file.ReadAllTextAsync();
                    var fileLines = fileCopy.Split('\n');
                    var idLine = fileLines[0].Split(':');
                    idLine[1] = userId.ToString();
                    string lineString = idLine[0] + ':' + idLine[1];
                    fileLines[0] = lineString;
                    string contents = "";
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        contents += fileLines[i] + '\n';
                    }
                    System.Diagnostics.Debug.WriteLine(contents);
                    await file.WriteAllTextAsync(contents);

                    //write contents to file

                }

                await Navigation.PushModalAsync(new AlternateCustomizeProfilePage());

            }
            else
            {
                invalidSignup = true;
                invalidSignupText = validityText;
                this.displaySignUpContent();
            }


        }

        private void LoginB_Clicked(object sender, EventArgs e)
        {

            this.displayLoginContent();


        }
        private bool checkEmailValidity(string email)
        {
            bool validity = false;
            if (email != null)
            {
                var match = Regex.Match(email, @"[\w\d-\.]*@[\w\d-]{1,}\.[\w\d-]{2,}");
                validity = match.Success;

            }
            else
            {
                return false;
            }
            return true;
        }
        private async Task<string> checkUserPassValidity(string username, string password)
        {
            string[] invalidChars = new string[13] { " ", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", ".", "," };
            bool login = await App.dbWrapper.LoginAccount(username, password);

            if (username != null)
            {
                if (login == true)
                {
                    return "Username Exists";
                }
                else
                {
                    if (!this.checkNameValidity(username).Equals("valid"))
                    {
                        return (checkNameValidity(username));
                    }
                    else if (username.Length < 5)
                    {
                        return "Username Too Short";
                    }
                    else
                    {
                        for (int i = 0; i < invalidChars.Length; i++)
                        {
                            if (username.Contains(invalidChars[i]))
                            {
                                return "Username Contains Invalid Characters";
                            }
                        }
                    }


                }


            }
            if (password != null)
            {
                if (password.Length < 7)
                {
                    return "Password Must Be Greater Than 6 Characters";
                }

            }

            return "valid";

        }
        private string checkNameValidity(string username)
        {
            string badWords = "pussy cock penis fuck porn sex vagina cum cunt orgy";
            var badWordsList = badWords.Split(' ');
            for (int i = 0; i < badWordsList.Length; i++)
            {
                if (username.Contains(badWordsList[i]))
                {
                    return "Invalid Name";
                }

            }
            return "Valid";

        }
    }
}