using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IPManager.Library.Integration.ExternalApi.Abstractions.Configuration;
using IPManager.Library.Integration.ExternalApi.Abstractions.ServiceClients;
using IPManager.Library.Integration.WebApi.Abstractions.RequestProvider;
using IPManager.Library.Models;

namespace IPManager.Library.Integration.ExternalApi.ServiceClients
{
    public class IPInfoProvider : IIPInfoProvider
    {
        private IRequestProvider _requestProvider;
        private readonly IPManagerConfig _ipManagerConfig;

        public IPInfoProvider(IRequestProvider requestProvider, IPManagerConfig config)
        {
            _requestProvider = requestProvider;
            _ipManagerConfig = config;
        }

        public async Task<IPDetails> GetDetails(string ip)
        {
            var uri = String.Format(_ipManagerConfig.ExternalApiUri, ip);
            return await _requestProvider.GetSingleItemRequest<IPDetails>(uri);
        }
    }
}
