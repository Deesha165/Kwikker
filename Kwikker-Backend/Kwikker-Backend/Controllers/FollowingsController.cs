using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.RequestFeatures;
using System.Text.Json;

namespace Kwikker_Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FollowingsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public FollowingsController(IServiceManager service) => _service = service;
        //follow

        [HttpPost("{followerId:int}/follows/{followeeId:int}")]
        public async Task<IActionResult> UserFollowOther(int followerId,int followeeId)
        {
            
          await _service.FollowService.CreateFollow(followerId, followeeId, trackChanges: false);
          
            return Ok();
        }
        //unfollow
        [HttpDelete("{followerId:int}/unfollows/{followeeId:int}")]
        public async Task<IActionResult> UserUnfollowOther(int followerId, int followeeId)
        {
           await _service.FollowService.DeleteFollow(followerId, followeeId, trackChanges: false);

            return Ok();
        }
        //get user followers
        [HttpGet("{followeeId:int}/followers")]
        public async Task<IActionResult> GetUserFollowers(int followeeId, [FromQuery] FollowingParameters followingParameters)
        {
           var pagedfollowers=  await  _service.FollowService.GetUserFollowers(followeeId, followingParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(pagedfollowers.metaData));

            return Ok(pagedfollowers.followers);
        }
        //get user followees
        [HttpGet("{followerId:int}/followees")]
        public async Task<IActionResult> GetUserFollowees(int followerId, [FromQuery] FollowingParameters followingParameters)
        {

            var pagedfollowees = await _service.FollowService.GetUserFollowees(followerId, followingParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(pagedfollowees.metaData));

            return Ok(pagedfollowees.followees);
        }
        [HttpGet("random/{UserId:int}")]
        public async Task<IActionResult>GetSuggestedToFollow(int UserId)
        {
            var suggestedUsers = await _service.FollowService.GetSuggestedToFollow(UserId);

            return Ok(suggestedUsers);
        }

    }
}
