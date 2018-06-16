using GarageForFriends.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageForFriends.Data
{
    public class MainPromoContainer : IContainer, IHeader, ITextBlock, IImage
    {
        public MainPromoContainer()
        {
            IdElement = ""; HeaderText = ""; RegularText = ""; Header = ""; ImgSource = ""; AltText = "";

        }

        public string IdElement { get; set; }
        public string HeaderText { get; set; }
        public string RegularText { get; set; }
        public string Header { get; set; }
        public string ImgSource { get; set; }
        public string AltText { get; set; }
    }
}
