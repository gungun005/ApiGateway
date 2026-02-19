using GreetingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace GreetingSystem.Controllers
{
    [ApiController]
    [Route("api/greet")]
    public class GreetController : ControllerBase
    {
        private readonly IGreetingService _greetingService;
        public GreetController(IGreetingService greetingService)
        {
            _greetingService = greetingService;

        }
        [HttpGet]
        public IActionResult Get()
        {
            var message = _greetingService.GetGreeting();
            return Ok(new { message });
        }

    }
}
