using IPManager.WebApi.Data.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using IPManager.WebApi.Data.Abstractions.Entities;
using IPManager.WebApi.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IPManager.WebApi.Data.Repositories
{
    public class BatchDetailsRepository : IBatchDetailsRepository
    {
        private readonly IPManagerDBContext _db;
        private readonly ILogger<BatchDetailsRepository> _logger;

        public BatchDetailsRepository(IPManagerDBContext context, ILogger<BatchDetailsRepository> logger)
        {
            _db = context;
            _logger = logger;
        }
        public async Task<(byte BatchStatus, int TotalBatchItems, int TotalBatchItemsSucceeded)> GetBatchProgressAsync(Guid guid)
        {
            var batch = await _db.Batch.FirstAsync(b => b.Guid == guid);
            var totalBatchItemsSucceeded = _db.BatchIPDetails.Count(b => b.BatchId == batch.Id);

            return (batch.StatusId, batch.TotalBatchItems, totalBatchItemsSucceeded);
        }

        public async Task InsertBatchAsync(byte batchStatus, Guid guid, int totalBatchItems)
        {
            var dto = new BatchDto()
            {
                StatusId = batchStatus,
                Guid = guid,
                CreatedAt = DateTime.UtcNow,
                TotalBatchItems = totalBatchItems
            };

            await _db.Batch.AddAsync(dto);
            await _db.SaveChangesAsync();
        }

        public async Task InsertBatchDetailsAsync(Guid guid, IEnumerable<string> ips)
        {
            var batchId = (await _db.Batch.FirstAsync(b => b.Guid == guid)).Id;

            foreach (var ip in ips)
            {
                var dto = new BatchIPDetailsDto()
                {
                    BatchId = batchId,
                    CreatedAt = DateTime.UtcNow,
                    IP = ip
                };
                await _db.AddAsync(dto);
            }
            await _db.SaveChangesAsync();
        }

        public async Task UpdateBatchStatusAsync(byte batchStatus, Guid guid)
        {
            var batch = await _db.Batch.FirstAsync(b => b.Guid == guid);
            batch.StatusId = batchStatus;
            _db.Batch.Update(batch);
            await _db.SaveChangesAsync();
        }
    }
}