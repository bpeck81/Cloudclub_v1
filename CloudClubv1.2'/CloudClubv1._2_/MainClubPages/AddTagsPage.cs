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
    public class AddTagsPage : ContentPage
    {
        public List<FrontTag> addedTags = new List<FrontTag>();
       public Entry tagEntry;

        ColorHandler ch;
        public AddTagsPage()
        {
          //  addedTags = new List<FrontTag>();
            ch = new ColorHandler();
            tagEntry = new Entry
            {
                Placeholder = "Add Tags",
                BackgroundColor = Color.White,
                TextColor = Color.Black
            };
            tagEntry.Completed += TagEntry_Completed;
            ListView listView = new ListView
            {

                ItemsSource = addedTags,
                ItemTemplate = new DataTemplate(typeof(TagViewCell))

            };

            Content = new StackLayout
            {
                Children = {
                    tagEntry,
                    listView

                },
                BackgroundColor = ch.fromStringToColor("gray")
            };
        }



        private void TagEntry_Completed(object sender, EventArgs e)
        {
            var tagE = (Entry)sender;

            if (addedTags.Count < 5) addedTags.Add(new FrontTag(tagE.Text));
            CreateClubPage.bAddTags.Text = addedTags.Count.ToString();
        }
    }
}
