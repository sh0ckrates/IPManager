using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
         
        public IPController(ILogger<IPController> logger)//testee
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string ip)
        {
            if (String.IsNullOrEmpty(ip)) return BadRequest(EmptyIPMessage);
            try
            {
                //var details = await _backlogsManager.GetBacklogAsync(backlogId);
                //return Ok(backlog);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ex.Message));
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
