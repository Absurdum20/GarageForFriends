using GarageForFriends.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using static GarageForFriends.Utils;

namespace GarageForFriends.Data
{
    public class PromoContainer : IContainer, IHeader, IListOfSlides<PromoContainer.Box>
    {
        public PromoContainer()
        {       
            IdElement = "";
            Header = "";
        }


        public string IdElement { get; set; }
        public int Width { get; set ; }
        public int Height { get; set; }
        public string Header { get; set; }
        
        public List<Box> Slides { get; set; }      

        public class Box : ITextBlock, IImage
        {
            public Box()
            {
                ImgSource = "";
                AltText = "";
                Width = 0;
                Height = 0;
                HeaderText = "";
                RegularText = "";
            }
            public string ImgSource { get; set; }
            public string AltText { get; set; }

            public int Width { get; set; }
            public int Height { get; set; }

            public string HeaderText { get; set; }
            public string RegularText { get; set; }
        }


    }

    public interface IListOfElements
    {
        List<UIElement> ListElements { get; set; }
    }
}
