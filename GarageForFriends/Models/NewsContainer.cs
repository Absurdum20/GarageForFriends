using GarageForFriends.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GarageForFriends.Data
{
    public class NewsContainer : IContainer, IHeader, IListOfSlides<NewsContainer.News>
    {
        public NewsContainer()
        {
            Slides = new List<News>();
            Header = "";
            IdElement = "";
        }

        public string IdElement { get; set; }
        public List<News> Slides { get; set; }
        public string Header { get; set; }

        public class News : ITextBlock, IImage, ISize
        {
            public News()
            {
                ImgSource = "";
                AltText = "";
                HeaderText = "";
                RegularText = "";
            }
            public int Width { get; set; }
            public int Height { get; set; }

            public string ImgSource { get; set; }
            public string AltText { get; set; }

            public string HeaderText { get; set; }
            public string RegularText { get; set; }
        }
    }
}