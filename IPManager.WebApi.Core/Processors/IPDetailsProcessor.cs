using IPManager.Library.Models;
using IPManager.WebApi.Core.Abstractions.Processors;
using IPManager.WebApi.Data.Abstractions.CacheProvider;
using System.Threading.Tasks;
using IPManager.WebApi.Data.Abstractions.Repositories;
using System;
using IPManager.Library.Integration.ExternalApi.Abstractions.Configuration;
using IPManager.Library.Integration.WebApi.Abstractions.RequestProvider;

namespace IPManager.WebApi.Core.Processors
{
    public class IPDetailsProcessor : IIPDetailsProcessor
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IIPDetailsRepository _detailsRepository;
        private IRequestProvider _requestProvider;
        private readonly IPManagerConfig _ipManagerConfig;

        public IPDetailsProcessor(ICacheProvider cacheProvider, IIPDetailsRepository detailsRepository,
            IRequestProvider requestProvider, IPManagerConfig config)
        {
            _cacheProvider = cacheProvider;
            _detailsRepository = detailsRepository;
            _requestProvider = requestProvider;
            _ipManagerConfig = config;
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

                result = await _detailsRepository.GetIPDetailsAsync(ip);
                _cacheProvider.SetCache(ip, result);
            }
            return _cacheProvider.GetFromCache<IPDetails>(ip);
        }


        public async Task<IPDetails> GetExternalApiDetails(string ip)
        {
            var uri = String.Format(_ipManagerConfig.ExternalApiUri, ip);
            var details = await _requestProvider.GetSingleItemRequest<IPDetails>(uri);
            await _detailsRepository.InsertIPDetailsAsync(ip, details);
            _cacheProvider.SetCache(ip,details);
            return details;
        }
    }
}
