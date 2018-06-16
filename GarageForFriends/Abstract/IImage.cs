using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageForFriends.Abstract
{
    public interface IImage
    {
        string ImgSource { get; set; }
        string AltText { get; set; }
    }
}
