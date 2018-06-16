using GarageForFriends.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageForFriends.Data
{
    public class HeaderContainer : IContainer, IHeader, IImage, ISize, IContacts, ISocial
    {
        public HeaderContainer()
        {
            AltPhone = ""; Phone = ""; Adress = "";
            VKHref = ""; YouTubeHref = ""; MapHref = "";
            ImgSource = ""; AltText = "";
        }

        public string IdElement { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }


        public string Email { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string Adress { get; set; }

        public string VKHref { get; set; }
        public string YouTubeHref { get; set; }
        public string MapHref { get; set; }

        public string Header { get; set; }
        public string ImgSource { get; set; }
        public string AltText { get; set; }
        
    }
}
