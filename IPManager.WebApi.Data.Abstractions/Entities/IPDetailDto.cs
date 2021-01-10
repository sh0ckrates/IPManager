using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Newtonsoft.Json;

namespace IPManager.WebApi.Data.Abstractions.Entities
{
    public class IPDetailsDto
    {
        public int Id { get; set; }

        public string Ip { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Continent { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
