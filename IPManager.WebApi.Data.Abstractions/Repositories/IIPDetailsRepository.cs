using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IPManager.Library.Models;
using IPManager.WebApi.Data.Abstractions.Entities;

namespace IPManager.WebApi.Data.Abstractions.Repositories
{
    public interface IIPDetailsRepository
    {
        Task<IPDetailsDto> GetIPDetailsAsync(string ip);
        Task InsertIPDetailsAsync(IPDetailsDto details);
        Task<bool> CheckIpExistenceAsync(string ip);
    }
}
