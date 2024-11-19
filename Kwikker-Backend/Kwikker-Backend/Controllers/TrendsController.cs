using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace Kwikker_Backend.Controllers
{
    [Route("api/[controller]")]
  
    [ApiController]
    public class TrendsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public TrendsController(IServiceManager service) => _service = service;
        [HttpGet("TopTrends")]
        public IActionResult GetTopTrends()
        {

            var trends = _service.trendService.GetTrends();

            return Ok(trends);
        }
        [HttpGet("{hashtag}")]
        [Authorize]
        public async Task<IActionResult> GetTweetsByTrend(string hashtag)
        
        {
           var tweets= await _service.trendService.GetTweetsByTrend(hashtag);
           
            return Ok(tweets);
        }

    }
}
