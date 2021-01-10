using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPManager.WebApi.Data.Abstractions.Repositories
{
    public interface IBatchDetailsRepository
    {
        Task InsertBatchAsync(byte batchStatus, Guid guid, int totalBatchItems);
        Task UpdateBatchStatusAsync(byte batchStatus, Guid guid);
        Task InsertBatchDetailsAsync(Guid guid, IEnumerable<string> ips);
        Task<(byte BatchStatus, int TotalBatchItems, int TotalBatchItemsSucceeded)> GetBatchProgressAsync(Guid guid);
    }
}