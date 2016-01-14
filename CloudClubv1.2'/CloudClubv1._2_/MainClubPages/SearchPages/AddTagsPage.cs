using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Backend;
using CloudClubv1._2_;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace FrontEnd
{
    public class AddTagsPage : ContentPage
    {
        public ObservableCollection<FrontTag> addedTags; 
        public Entry tagEntry;
        ListView listView;
        ColorHandler ch;
        public AddTagsPage()
        {
            addedTags = new ObservableCollection<FrontTag>();
            //  addedTags = new List<FrontTag>();
            ch = new ColorHandler();
            Title = "Add Club Tags";

            MessagingCenter.Subscribe<TagViewCell, string>(this, "Remove Tag", (sender, arg) => {
                var tagName = arg as string;
                for(int i =0; i<addedTags.Count; i++)
                {
                    if(addedTags[i].Tag == tagName)
                    {
                        addedTags.RemoveAt(i);
                    }
                }
            });

            tagEntry = new Entry
            {
                Placeholder = "Add Tags",
                BackgroundColor = ch.fromStringToColor("purple"),
                TextColor = ch.fromStringToColor("white"),
                
            };
            tagEntry.Completed += TagEntry_Completed;
            listView = new ListView
            {

                ItemsSource = addedTags,
                ItemTemplate = new DataTemplate(typeof(TagViewCell)),
                SeparatorColor = ch.fromStringToColor("lightGray"),
             };
      
            listView.ItemTapped += (sender, args) =>
             {
                 var sentTag = args.Item as FrontTag;
                 System.Diagnostics.Debug.WriteLine(sentTag.Tag);

                 foreach (FrontTag ft  in addedTags)
                 {
                     if(ft.removeTag)
                     {
                         ft.removeTag = false;
                         ft.UpdateProperty("removeTag");
                     }
                     if (ft.Tag.Equals(sentTag.Tag)){
                         ft.removeTag = true;
                         ft.UpdateProperty("removeTag");                         
                     }
                 }
                
                  
             };
            Content = new StackLayout
            {
                Children = {
                    tagEntry,
                    listView

                },
                BackgroundColor = ch.fromStringToColor("white")
            };
        }



        private void TagEntry_Completed(object sender, EventArgs e)
        {
            var tagE = (Entry)sender;

            if (addedTags.Count < 5)
            {
                addedTags.Add(new FrontTag(tagE.Text));
                MessagingCenter.Send<AddTagsPage, string>(this, "Tag Added", tagE.Text);
            }
            tagE.Text = "";

            //CreateClubPage.bAddTags.Text = addedTags.Count.ToString();
        }
    }
}
