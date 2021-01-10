using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IPManager.Library.Models;

namespace IPManager.WebApi.Data.Abstractions.Repositories
{
    public interface IIPDetailsRepository
    {
        Task<IPDetails> GetIPDetailsAsync(string ip);
        Task InsertIPDetailsAsync(IPDetails details);
        Task<bool> CheckIpExistenceAsync(string ip);
    }
}
