using IPManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPManager.WebApi.Core.Abstractions.Providers
{
    public interface IIPInfoProvider
    {
        Task<IPDetails> GetDetails(string ip);
        Task<Guid> CreateBatchAsync(int totalBatchItems);
        Task UpdateIPDetailsAsync(Guid guid, IEnumerable<IPDetails> ipDetailsList);
        Task<string> GetBatchProgressAsync(Guid guid);
    }
}
