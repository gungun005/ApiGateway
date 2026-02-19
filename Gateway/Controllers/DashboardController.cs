using Gateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardAggregatorService _aggregator;
        public DashboardController(DashboardAggregatorService aggregator)
        {
            _aggregator = aggregator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Dashboard endpoint hit");
            var result = await _aggregator.GetDashboardAsync();
            return Ok(result);
        }

    }
}
