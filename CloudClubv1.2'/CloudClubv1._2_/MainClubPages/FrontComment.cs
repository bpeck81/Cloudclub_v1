using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using CloudClubv1._2_;

namespace FrontEnd
{
    public class FrontComment
    {

        public string Id { get; set; }

        public DateTime? Time { get; set; }

        public string AuthorId { get; set; }

        public string Text { get; set; }

        public int NumDroplets { get; set; }

        public string ClubId { get; set; }

        public bool Picture { get; set; }

        public string UserEmoji { get; set; }
        
        public string AuthorAccountColor { get; set; }

        public string TextColor { get; set; }

        public FrontComment(Comment comment, Account authorAccount)
        {
            Id = comment.Id;
            Time = comment.Time;
            Text = comment.Text;
            NumDroplets = comment.NumDroplets;
            ClubId = comment.ClubId;
            Picture = comment.Picture;
            UserEmoji = authorAccount.Emoji;
            AuthorAccountColor = authorAccount.Color;
            TextColor = "black";
            
            if(comment.AuthorId == App.dbWrapper.GetUser().Id){
                AuthorId = "You";
                TextColor = AuthorAccountColor;
            }
            else
            {
                AuthorId = comment.AuthorId;
            }
            

        }

    }
}
