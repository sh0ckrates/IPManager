﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IPManager.WebApi.Models
{
    public class IPDetails
    {
       public string City { get; set; }
       public string Country { get; set; }
       public string Continent { get; set; }
       public double Latitude { get; set; }
       public double Longitude { get; set; }
    }
}
