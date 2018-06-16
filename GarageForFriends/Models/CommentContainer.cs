using GarageForFriends.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageForFriends.Data
{
    public class CommentContainer : IContainer, IHeader, IListOfSlides<CommentContainer.Comment>
    {
        public CommentContainer()
        {
            IdElement = ""; Header = "";
            Slides = new List<Comment>();
        }

        public string IdElement { get; set; }
        public string Header { get; set; }
        public List<Comment> Slides { get; set; }

        public class Comment : IImage, ITextBlock
        {
            public Comment() {
                ImgSource = "";
                AltText = "";
                HeaderText = "";
                RegularText = "";
            }
            public string ImgSource { get; set; }
            public string AltText { get; set; }

            public string HeaderText { get; set; }
            public string RegularText { get; set; }
        }
    }
}
