using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TraefikPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EndpointsController : ControllerBase
    {
        private readonly ILogger<EndpointsController> _logger;
        private readonly IDashboardItemProvider provider;

        public EndpointsController(ILogger<EndpointsController> logger, IDashboardItemProvider provider)
        {
            _logger = logger;
            this.provider = provider;
        }

        [HttpGet]
        public IEnumerable<DashboardItem> Get()
        {
            return provider.GetDashboardItems();
        }
    }
}
