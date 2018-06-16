using GarageForFriends.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageForFriends.Data
{
    public class ServicesContainer : IContainer, IHeader, IListOfSlides<ServicesContainer.Service>
    {
        public ServicesContainer()
        {
            Slides = new List<Service>();
            Header = "";
            IdElement = "";
        }

        public string IdElement { get; set; }
        public string Header { get; set; }

        public List<Service> Slides { get; set; }

        public class Service : IImage, ITextBlock, IPrice
        {
            public Service()
            {
                ImgSource = "";
                AltText = "";
                HeaderText = "";
                RegularText = "";
            }

            public string ImgSource { get; set; }
            public string AltText { get; set; }

            public string HeaderText { get; set; }
            public string RegularText { get; set; }

            public int Price { get; set; }
        }
    }
}
