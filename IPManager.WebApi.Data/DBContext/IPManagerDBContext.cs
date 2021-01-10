using IPManager.WebApi.Data.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace IPManager.WebApi.Data.DBContext
{
    public class IPManagerDBContext : DbContext
    {
        public IPManagerDBContext(DbContextOptions<IPManagerDBContext> options)
            : base(options)
        {

        }

        public DbSet<IPDetailsDto> IPDetail { get; set; }
        public DbSet<BatchDto> Batch { get; set; }
        public DbSet<BatchIPDetailsDto> BatchIPDetails { get; set; }
    }
}