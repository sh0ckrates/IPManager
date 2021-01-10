using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IPManager.Library.Models;

namespace IPManager.Library.Integration.ExternalApi.Abstractions.ServiceClients
{
    public interface IIPInfoManager
    {
        Task<IPDetails> GetDetails(string ip);
    }
}
