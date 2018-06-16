using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageForFriends.Data
{
    public interface ITextBlock
    {
        string HeaderText { get; set; }
        string RegularText { get; set; }
    }
}
