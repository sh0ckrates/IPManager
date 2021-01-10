using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using IPManager.Library.Models;
using IPManager.WebApi.Data.Abstractions.Entities;

namespace IPManager.WebApi.Core.Abstractions.Configuration
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
