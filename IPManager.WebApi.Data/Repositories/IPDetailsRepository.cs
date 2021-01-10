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

        public async Task MergeIPDetailsAsync(IEnumerable<IPDetailsDto> ipDetailsDtos)
        {
            foreach (var dto in ipDetailsDtos)
            {
                if (!await _db.IPDetail.AnyAsync(i => i.Ip == dto.Ip))
                {
                    await InsertIPDetailsAsync(dto);
                }
                else
                {
                    var ipDetails = await _db.IPDetail.FirstAsync(d => d.Ip == dto.Ip);
                    ipDetails.City = dto.City;
                    ipDetails.Country = dto.Country;
                    ipDetails.Continent = dto.Continent;
                    ipDetails.Longitude = dto.Longitude;
                    ipDetails.Latitude = dto.Latitude;
                    _db.IPDetail.Update(ipDetails);
                }

            }
            await _db.SaveChangesAsync();
        }

        public async Task<IPDetailsDto> GetIPDetailsAsync(string ip)
        {
            IQueryable<IPDetailsDto> query = _db.IPDetail;
            query = query.Where(a => a.Ip == ip);
            return await query.FirstOrDefaultAsync();
        }


        public async Task InsertIPDetailsAsync(IPDetailsDto details) 
        {
            await _db.AddAsync<IPDetailsDto>(details);
            await _db.SaveChangesAsync();
        }
    }
}