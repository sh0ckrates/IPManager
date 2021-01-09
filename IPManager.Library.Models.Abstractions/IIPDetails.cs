using System;
using System.Collections.Generic;
using System.Text;

namespace IPManager.Library.Models.Abstractions
{
    public interface IIPDetails
    {
        string City { get; set; }
        string Country { get; set; }
        string Continent { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
    }
}
