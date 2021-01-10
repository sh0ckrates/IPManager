using Microsoft.AspNetCore.Mvc;

namespace IPManager.WebApi.Controllers
{
    [ApiController]
    [Route("api/ipmanager/[controller]")]
    public class IPControllerBase : ControllerBase
    {
        protected const string EmptyIPMessage = "No ip specified on request.";
        protected const string EmptyRequest = "The request is empty.";
    }
}
