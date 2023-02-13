namespace Rate_limiting_Example.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.RateLimiting;
    using System.Threading.RateLimiting;
    using Rate_limiting_Example.Services;

    [ApiController]
    [EnableRateLimiting("customLimiter")]
    public class TimeController : Controller
    {
        private readonly IGetTimeService _service;

        public TimeController(IGetTimeService service)
        {
            _service = service;
        }

        //[HttpGet]
        //[Route("[controller]/time-current")]
        //[EnableRateLimiting("LimiterPolicy")]
        //public async Task<IActionResult> Index()
        //{
        //    await Task.Delay(2000);
        //    return Ok(_service.currentTime());
        //}

        [HttpGet]
        [Route("[controller]/time-current")]
        //[EnableRateLimiting("LimiterPolicy")]
        public IActionResult Index()
        {
            return Ok(_service.currentTime());
        }
    }
}
