using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using CloudClubv1._2_;
using System.ComponentModel;

namespace FrontEnd
{
    public class FrontComment: INotifyPropertyChanged
    {

        //declares an event
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUsername { get; set; }

        public string Text { get; set; }

        public int NumDroplets { get; set; }

        public string ClubId { get; set; }

        public bool Picture { get; set; }

        public string UserEmoji { get; set; }
        
        public string AuthorAccountColor { get; set; }

        public string TextColor { get; set; }

        public string ClubRequestText { get; set; }

        public bool ClubRequestBool { get; set; }
        
        public string ClubRequestUsername { get; set; }
        
        public ClubRequest ClubRequestInstance { get; set; }

        public bool IsMember { get; set; }


        public FrontComment(Comment comment, Account authorAccount)
        {
            Id = comment.Id;
            IsMember = false;
            ClubRequestBool = false;
            Time = comment.Time;
            Text = comment.Text;
            NumDroplets = comment.NumDroplets;
            ClubId = comment.ClubId;
            Picture = comment.Picture;
            UserEmoji = authorAccount.Emoji;
            AuthorAccountColor = authorAccount.Color;
            TextColor = "black";
            
            if(comment.AuthorId == App.dbWrapper.GetUser().Id){
                AuthorUsername = "You";
                TextColor = AuthorAccountColor;
            }
            else
            {
                AuthorUsername = authorAccount.Username;
            }
            

        }
        public FrontComment(ClubRequest clubRequest, Account authorAccount, bool isMember)
        {
            this.IsMember= isMember;
            ClubRequestBool = true;
            ClubId = clubRequest.ClubId;
            ClubRequestText = clubRequest.Text;
            ClubRequestUsername = authorAccount.Username;
            ClubRequestInstance = clubRequest;
            
          
        }

        //raise the event, which will cause the class to update
        public void UpdateProperty(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null){
                handler(this, new PropertyChangedEventArgs(propertyName));
            }

        }

    }
}
