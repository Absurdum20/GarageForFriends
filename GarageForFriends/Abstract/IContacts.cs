using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GarageForFriends.Abstract
{
    public interface IContacts
    {
        string Email { get; set; }
        string Phone { get; set; }
        string AltPhone { get; set; }
        string Adress { get; set; }
    }
}
