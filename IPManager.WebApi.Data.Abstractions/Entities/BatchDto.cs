using System;

namespace IPManager.WebApi.Data.Abstractions.Entities
{
    public class BatchDto
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte StatusId { get; set; }
        public int TotalBatchItems { get; set; }
    }
}
