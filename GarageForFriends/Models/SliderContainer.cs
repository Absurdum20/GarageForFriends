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
    public class SliderContainer : IContainer, IHeader, IListOfSlides<SliderContainer.Slide>
    {
        public SliderContainer()
        {
            Slides = new List<Slide>();
            Header = "";
            IdElement = "";
        }

        public string IdElement { get; set; }
      
        public List<Slide> Slides { get; set; }
        public string Header { get; set; }


        public class Slide : IImage
        {
            public Slide()
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
        }
    }
}
