using GarageForFriends.Data;
using System.Collections.Generic;

namespace GarageForFriends.Abstract
{
    public interface IListOfSlides<T>
    {
        List<T> Slides { get; set; }
    }
}