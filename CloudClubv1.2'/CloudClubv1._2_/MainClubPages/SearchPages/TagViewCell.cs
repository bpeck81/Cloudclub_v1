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

        public TagViewCell()
        {
            Label tagL = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            tagL.SetBinding(Label.TextProperty, "Tag");

            View = new StackLayout
            {
                Children =
                {
                    tagL
                },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White


            };

        }
    }
}
