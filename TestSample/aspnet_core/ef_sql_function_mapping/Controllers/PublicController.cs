using EF_Test.Services;
using EF_Test.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EF_Test.Controllers
{
    [Route("public/[action]")]
    [ApiController]
    public class PublicController : Controller
    {
        private readonly IRevenueService _revenueService = null!;
        private readonly IConfiguration _configuration = null!;

        public PublicController(IRevenueService revenueService,
            IConfiguration configuration)
        {
            _revenueService = revenueService;
            _configuration = configuration;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetRealizedPlByAll()
        //{
        //    var allRealizedPl = await _revenueService.GetAllListAsync();
        //    return Ok(allRealizedPl);
        //}

        [HttpGet]
        public async Task<IActionResult> FindRealizedPl_Error(DateTime createDate)
        {
            var allRealizedPl = await _revenueService.GetAllListAsync_Error(DateOnly.FromDateTime(createDate));
            return Ok(allRealizedPl);
        }

        [HttpGet]
        public async Task<IActionResult> FindRealizedPl01(DateTime createDate)
        {
            var allRealizedPl = await _revenueService.GetAllListAsync_Cast_Ver1(createDate);
            return Ok(allRealizedPl);
        }

        [HttpGet]
        public async Task<IActionResult> FindRealizedPl02(DateTime createDate)
        {
            var allRealizedPl = await _revenueService.GetAllListAsync_Cast_Ver2(DateOnly.FromDateTime(createDate));
            return Ok(allRealizedPl);
        }

        [HttpGet]
        public async Task<IActionResult> FindRealizedPl03(DateTime createDate)
        {
            var allRealizedPl = await _revenueService.GetAllListAsync_Cast_Ver3(createDate);
            return Ok(allRealizedPl);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRealizedPl(RealizedPlViewModel realizedPl)
        {
            var createdRealizedPl = await _revenueService.CreateAsync(realizedPl);
            return Ok(createdRealizedPl);
        }
    }
}
