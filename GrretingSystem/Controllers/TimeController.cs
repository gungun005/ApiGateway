using Microsoft.AspNetCore.Mvc;
using TimeService.Services;

namespace TimeService.Controllers
{
    [ApiController]
    [Route("api/time")]
    public class TimeController : ControllerBase
    {
        private readonly ITimeService _timeService;
        public TimeController(ITimeService timeService)
        {
            _timeService = timeService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var time = _timeService.GetTime();

            return Ok(new
            {
                Time = time
            });
        }

    }
}
