using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IPManager.WebApi.Controllers
{
    [ApiController]
    public class IPControllerBase : ControllerBase
    {
        protected const string EmptyIPMessage = "No ip specified on request.";
    }
}
