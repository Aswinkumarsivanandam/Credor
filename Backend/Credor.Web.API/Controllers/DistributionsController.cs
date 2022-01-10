using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Credor.Web.API.Controllers
{
    [Route("Distributions")]
    [Produces("application/json")]
    [ApiController]
    public class DistributionsController : ControllerBase
    {
        private readonly IDistributionsService _distributionsService;
        public IConfiguration _configuration { get; }

        public DistributionsController(IDistributionsService distributionsService, IConfiguration configuration)
        {
            _configuration = configuration;
            _distributionsService = distributionsService;
        }

        [HttpGet]
        [Route("getalldistributions/{userid}")]
        public IActionResult GetAllDistributions(int userid)
        {
            var distributions = _distributionsService.GetAllDistributions(userid);
            return Ok(distributions);
        }

    }
}
