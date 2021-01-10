using IPManager.Library.Models;
using IPManager.WebApi.Data.Abstractions.CacheProvider;
using System.Threading.Tasks;
using IPManager.WebApi.Data.Abstractions.Repositories;
using System;
using AutoMapper;
using IPManager.Library.Integration.ExternalApi.Abstractions.Configuration;
using IPManager.Library.Integration.WebApi.Abstractions.RequestProvider;
using IPManager.WebApi.Core.Abstractions.Providers;
using IPManager.WebApi.Data.Abstractions.Entities;

namespace IPManager.WebApi.Core.Providers
{
    public class IPInfoProvider : IIPInfoProvider
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IIPDetailsRepository _detailsRepository;
        private IRequestProvider _requestProvider;
        private IMapper _mapper;
        private readonly IPManagerConfig _ipManagerConfig;

        public IPInfoProvider(ICacheProvider cacheProvider, IIPDetailsRepository detailsRepository,
            IRequestProvider requestProvider, IPManagerConfig config, IMapper mapper)
        {
            _cacheProvider = cacheProvider;
            _detailsRepository = detailsRepository;
            _requestProvider = requestProvider;
            _ipManagerConfig = config;
            _mapper = mapper;
        }

        public async Task<IPDetails> GetDetails(string ip)
        {
            var details = await GetStoredDetails(ip);
            details ??= await GetExternalApiDetails(ip);
            return details;
        }


        public async Task<IPDetails> GetStoredDetails(string ip)
        {
            IPDetails result;
            bool keyExistsInCache = _cacheProvider.KeyExists<IPDetails>(ip);
            
            if (!keyExistsInCache)
            {
                bool keyExistsInDb = await _detailsRepository.CheckIpExistenceAsync(ip);
                if (!keyExistsInDb) return null;

                var detailsDto = await _detailsRepository.GetIPDetailsAsync(ip);
                result = _mapper.Map<IPDetails>(detailsDto);

                _cacheProvider.SetCache(ip, result);
            }
            return _cacheProvider.GetFromCache<IPDetails>(ip);
        }


        public async Task<IPDetails> GetExternalApiDetails(string ip)
        {
            var uri = String.Format(_ipManagerConfig.ExternalApiUri, ip);
            var details = await _requestProvider.GetSingleItemRequest<IPDetails>(uri);
            var detailsDto = new IPDetailsDto()
            {
                Ip = details.Ip,
                City = details.City,
                Country = details.Country,
                Continent = details.Continent,
                Longitude = details.Longitude,
                Latitude = details.Latitude
            };       //_mapper.Map<IPDetailsDto>(details);
            await _detailsRepository.InsertIPDetailsAsync(detailsDto);
            _cacheProvider.SetCache(ip,details);
            return details;
        }
    }
}
