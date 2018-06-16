using GarageForFriends.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using static GarageForFriends.Utils;

namespace GarageForFriends.Data
{
    public class YoutubeContainer : IContainer, IHeader, IListOfSlides<YoutubeContainer.Youtube>
    {
        public YoutubeContainer()
        {
            IdElement = "";
            Slides = new List<Youtube>();
        }

        public string IdElement { get; set; }
        public string Header { get; set; }
        public List<Youtube> Slides { get; set; }


        public class Youtube : ITextBlock, IGoogleVideo
        {
            public Youtube()
            {
                HrefToGoogle = "";
                HeaderText = "";
                RegularText = "";
            }

            public string HrefToGoogle { get; set; }

            public string HeaderText { get; set; }
            public string RegularText { get; set; }
        }
    }
}
