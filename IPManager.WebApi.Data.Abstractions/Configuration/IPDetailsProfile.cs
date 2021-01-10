using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using IPManager.Library.Models;
using IPManager.Library.Models.Abstractions;
using IPManager.WebApi.Data.Abstractions.Entities;

namespace IPManager.WebApi.Data.Abstractions.Configuration
{
    public class IPDetailsProfile : Profile
    {
        public IPDetailsProfile()
        {
            CreateMap<IPDetails, IPDetailsDto>()
                .ReverseMap();
        }
    }

}
