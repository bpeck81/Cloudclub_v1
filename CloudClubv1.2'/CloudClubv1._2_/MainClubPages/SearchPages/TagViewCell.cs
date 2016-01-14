using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrontEnd
{
    class TagViewCell: ViewCell
    {
        TapGestureRecognizer removeTgr;
        ColorHandler ch;
        Label tagL;
        public TagViewCell()
        {
            removeTgr = new TapGestureRecognizer();
            removeTgr.Tapped += RemoveTgr_Tapped;
            ch = new ColorHandler();
             tagL = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = ch.fromStringToColor("black")
            };
            tagL.SetBinding(Label.TextProperty, "Tag");
            var lRemove = new Label
            {
                Text = "Remove Tag",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = ch.fromStringToColor("gray"),
                HorizontalOptions=  LayoutOptions.CenterAndExpand
            };
            lRemove.SetBinding(Label.IsVisibleProperty, "removeTag");
            lRemove.GestureRecognizers.Add(removeTgr);
            View = new StackLayout
            {
                Children =
                {
                    tagL,
                    lRemove
                },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Spacing= 5,
                BackgroundColor = Color.White


            };

        }

        private void RemoveTgr_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send<TagViewCell, string>(this, "Remove Tag", tagL.Text);
        }
    }
}
