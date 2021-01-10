using System;

namespace IPManager.WebApi.Data.Abstractions.Entities
{
    public class BatchIPDetailsDto
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public string IP { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
