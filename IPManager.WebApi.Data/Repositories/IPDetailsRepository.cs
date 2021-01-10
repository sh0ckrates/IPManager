using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IPManager.Library.Models;
using IPManager.WebApi.Data.Abstractions.Entities;
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
            return _db.IPDetail.Any(o => o.Ip == ip);
        }


        public async Task<IPDetailsDto> GetIPDetailsAsync(string ip)
        {
            IQueryable<IPDetailsDto> query = _db.IPDetail;
            query = query.Where(a => a.Ip == ip);
            return await query.FirstOrDefaultAsync();
        }


        public async Task InsertIPDetailsAsync(IPDetailsDto details) 
        {
            _db.Add<IPDetailsDto>(details);
            _db.SaveChanges();
        }

        //public void Add<T>(T entity) where T : class
        //{
        //    _context.Add(entity);
        //}

    }
}
