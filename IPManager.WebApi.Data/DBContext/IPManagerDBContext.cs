using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using IPManager.Library.Models;
using Microsoft.EntityFrameworkCore;

namespace IPManager.WebApi.Data.DBContext
{
    public class IPManagerDBContext : DbContext
    {
        public IPManagerDBContext(DbContextOptions<IPManagerDBContext> options)
            : base(options)
        {

        }

        public DbSet<IPDetails> IPAddresses { get; set; }
    }

}
