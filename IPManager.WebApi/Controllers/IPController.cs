using IPManager.Library.Integration.ExternalApi.Abstractions.Exceptions;
using IPManager.Library.Models;
using IPManager.WebApi.Core.Abstractions.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPManager.WebApi.Controllers
{
    public class IPController : IPControllerBase  
    {
        private readonly ILogger<IPController> _logger;   
        private readonly IIPInfoProvider _infoProvider;   
        
         
        public IPController(ILogger<IPController> logger, IIPInfoProvider infoProvider)
        {
            _logger = logger;
            _infoProvider = infoProvider;
        }

        [HttpGet("{ip}")]
        public async Task<IActionResult> GetAsync(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return BadRequest(EmptyIPMessage);
            
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

        [HttpPost("update-batch")]
        public async Task<IActionResult> UpdateAsync([FromBody] IEnumerable<IPDetails> ipDetailsList)
        {
            if (ipDetailsList is null) return BadRequest(EmptyRequest);
            if (!ipDetailsList.Any()) return StatusCode(StatusCodes.Status204NoContent);
            
            try
            {
                var guid = await _infoProvider.CreateBatchAsync(ipDetailsList.Count());
                await _infoProvider.UpdateIPDetailsAsync(guid, ipDetailsList);
                return Accepted(guid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ex.Message));
                throw new IPServiceNotAvailableException(ControllerContext.ActionDescriptor.ActionName);
            }
        }

        [HttpGet("batch-progress")]
        public async Task<IActionResult> GetProgressAsyncAsync([FromQuery]string batchIdentifier)
        {
            if (string.IsNullOrWhiteSpace(batchIdentifier)) return BadRequest("No guid has been provided.");
            if (!Guid.TryParse(batchIdentifier, out var guid)) return BadRequest("No valid guid has been provided.");
            
            try
            {
                var batchProgress = await _infoProvider.GetBatchProgressAsync(guid);
                return Ok(batchProgress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ex.Message));
                throw new IPServiceNotAvailableException(ControllerContext.ActionDescriptor.ActionName);
            }
        }

    }
}
