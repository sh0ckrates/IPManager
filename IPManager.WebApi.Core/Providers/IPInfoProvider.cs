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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPManager.Library.Integration.ExternalApi.Abstractions.Exceptions;
using IPManager.WebApi.Core.Abstractions.Enums;
using IPManager.WebApi.Core.Abstractions.Configuration;
using Microsoft.Extensions.Logging;

namespace IPManager.WebApi.Core.Providers
{
    public class IPInfoProvider : IIPInfoProvider
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IIPDetailsRepository _detailsRepository;
        private readonly IBatchDetailsRepository _batchDetailsRepository;
        private IRequestProvider _requestProvider;
        private ILogger<IPInfoProvider> _logger;
        private IMapper _mapper;
        private readonly IPManagerConfig _ipManagerConfig;
        private readonly IPInfoProviderConfig _ipInfoProviderConfig;

        public IPInfoProvider(
            ICacheProvider cacheProvider, 
            IIPDetailsRepository detailsRepository,
            IBatchDetailsRepository batchDetailsRepository,
            IRequestProvider requestProvider,
            IPManagerConfig ipManagerConfig,
            IPInfoProviderConfig ipInfoProviderConfig,
            ILogger<IPInfoProvider> logger,
            IMapper mapper)
        {
            _cacheProvider = cacheProvider;
            _detailsRepository = detailsRepository;
            _batchDetailsRepository = batchDetailsRepository;
            _requestProvider = requestProvider;
            _ipManagerConfig = ipManagerConfig;
            _ipInfoProviderConfig = ipInfoProviderConfig;
            _logger = logger;
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
            var uri = string.Format(_ipManagerConfig.ExternalApiUri, ip);
            var details = await _requestProvider.GetSingleItemRequest<IPDetails>(uri);
            var detailsDto = _mapper.Map<IPDetailsDto>(details);
            await _detailsRepository.InsertIPDetailsAsync(detailsDto);
            _cacheProvider.SetCache(ip,details);
            return details;
        }

        public async Task<Guid> CreateBatchAsync(int totalBatchItems)
        {
            var guid = Guid.NewGuid();
            await _batchDetailsRepository.InsertBatchAsync((byte)BatchStatus.Initial, guid, totalBatchItems);
            return guid;
        }

        public async Task UpdateIPDetailsAsync(Guid guid, IEnumerable<IPDetails> ipDetailsList)
        {
            try
            {
                await _batchDetailsRepository.UpdateBatchStatusAsync((byte)BatchStatus.InProgress, guid);

                IEnumerable<IPDetails> ipDetailsPage;
                IEnumerable<IPDetailsDto> ipDetailsDtos;

                var totalItems = ipDetailsList.Count();
                var pageSize = _ipInfoProviderConfig.PageSize;
                var pageNumber = 0;
                var totalPages = (totalItems % pageSize == 0) ? totalItems / pageSize : totalItems / pageSize + 1;

                for (var i = 0; i < totalItems; i += pageSize)
                {
                    pageNumber++;
                    ipDetailsPage = ipDetailsList.Skip(i).Take(pageSize);
                    ipDetailsDtos = _mapper.Map<IEnumerable<IPDetailsDto>>(ipDetailsPage);
                    await _detailsRepository.MergeIPDetailsAsync(ipDetailsDtos);
                    await UpdateIPDetailsPageInCache(ipDetailsPage);
                    await _batchDetailsRepository.InsertBatchDetailsAsync(guid, ipDetailsPage.Select(i => i.Ip));

                    if (pageNumber == totalPages) await _batchDetailsRepository.UpdateBatchStatusAsync((byte)BatchStatus.Completed, guid);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UpdateIPDetailsAsync)} threw an exception with message: {ex.Message}.");
            }
        }

        private async Task UpdateIPDetailsPageInCache(IEnumerable<IPDetails> ipDetailsList)
        {
            foreach (var ipDetail in ipDetailsList)
            {
                _cacheProvider.SetCache(ipDetail.Ip, ipDetail);
            }
        }

        public async Task<string> GetBatchProgressAsync(Guid guid)
        {
            var progressResult = await _batchDetailsRepository.GetBatchProgressAsync(guid);
            var sb = new StringBuilder();

            sb.AppendLine($"Progress for GUID: {guid}");
            sb.AppendLine($"► Status: {(BatchStatus) progressResult.BatchStatus}");
            sb.AppendLine($"► Total batch items: {progressResult.TotalBatchItems}");
            sb.AppendLine($"► Successfully processed batch items: {progressResult.TotalBatchItemsSucceeded}, i.e. { ((double)(progressResult.TotalBatchItemsSucceeded / progressResult.TotalBatchItems) * 100).ToString("0.##")} %!");

            return sb.ToString();
        }
    }
}
