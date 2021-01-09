using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IPManager.Library.Integration.ExternalApi.Abstractions.Exceptions;
using IPManager.Library.Integration.ExternalApi.Abstractions.ServiceClients;
using IPManager.Library.Models.Abstractions;
using IPManager.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IPManager.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IPController : IPControllerBase  
    {
        private readonly ILogger<IPController> _logger;   
        private readonly IIPInfoProvider _infoProvider;   
        
         
        public IPController(ILogger<IPController> logger, IIPInfoProvider infoProvider)
        {
            _logger = logger;
            _infoProvider = infoProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string ip)
        {
            if (String.IsNullOrEmpty(ip)) return BadRequest(EmptyIPMessage);
            try
            {
                var details = await _infoProvider.GetDetails(ip);
                return Ok(details);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ex.Message));
                throw new IPServiceNotAvailableException(ControllerContext.ActionDescriptor.ActionName);
            }
        }

    }
}
