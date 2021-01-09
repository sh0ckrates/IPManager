using IPManager.Library.Models.Abstractions;

namespace IPManager.Library.Models
{
    public class IPDetails : IIPDetails
    {
       public string City { get; set; }
       public string Country { get; set; }
       public string Continent { get; set; }
       public double Latitude { get; set; }
       public double Longitude { get; set; }
    }
}
