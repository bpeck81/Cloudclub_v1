using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using Backend;

namespace FrontEnd
{
    public class FriendsClubListPage : ContentPage
    {
        public FriendsClubListPage(List<Club> clubList)
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}
