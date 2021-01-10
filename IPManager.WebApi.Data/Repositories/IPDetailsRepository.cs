using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IPManager.Library.Models;
using IPManager.WebApi.Data.Abstractions.Repositories;
using IPManager.WebApi.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IPManager.WebApi.Data.Repositories
{
    public class IPDetailsRepository : IIPDetailsRepository
    {

        private readonly IPManagerDBContext _db;
        private readonly ILogger<IPDetailsRepository> _logger;

        public IPDetailsRepository(IPManagerDBContext context, ILogger<IPDetailsRepository> logger)
        {
            _db = context;
            _logger = logger;
        }

        public async Task<bool> CheckIpExistenceAsync(string ip)
        {
            var exists = false;
            IQueryable<IPDetails> query = _db.IPAddresses;
            if (_db.IPAddresses.Any(o => o.Ip == ip)) exists = true;
            return exists;
        }


        public async Task<IPDetails> GetIPDetailsAsync(string ip)
        {
            IQueryable<IPDetails> query = _db.IPAddresses;
            query = query.Where(a => a.Ip == ip);
            return await query.FirstOrDefaultAsync();
        }


        public async Task InsertIPDetailsAsync(IPDetails details) 
        {
            _db.Add<IPDetails>(details);
            _db.SaveChanges();
        }

        //public void Add<T>(T entity) where T : class
        //{
        //    _context.Add(entity);
        //}

    }
}
