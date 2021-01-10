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
        [Key]
        [Column("IPAddress")]
        public string Ip { get; set; }

        [Column("City")]
        public string City { get; set; }

        [Column("Country")]
        public string Country { get; set; }

        [Column("Continent")]
        public string Continent { get; set; }

        [Column("Latitude")]
        public double? Latitude { get; set; }

        [Column("Longitude")]
        public double? Longitude { get; set; }
    }
}
