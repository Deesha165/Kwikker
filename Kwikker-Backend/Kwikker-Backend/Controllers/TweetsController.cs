using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetTopologySuite.Operation.Buffer;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Kwikker_Backend.Controllers
{
    [Route("api/[controller]")]
   
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public TweetsController(IServiceManager service) => _service = service;
        //Tweets Creation, Retreival and Removal

       
        [HttpGet("User/{UserId:int}")]
        public async Task<IActionResult> GetTweetsByUser(int UserId, [FromQuery] TweetParameters tweetParameters)
        {
            var pagedTweets = await _service.TweetService.GetTweetsByUser(UserId, tweetParameters,trackChanges: false);
            Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(pagedTweets.metaData));

            return Ok(pagedTweets.tweets);
        }

        [HttpPost("{UserId:int}")]
        public async Task<IActionResult> CreateTweet(int UserId,[FromBody] TweetForCreationDTO tweet)
        {
            if (tweet is null) return BadRequest("TweetForCreationDTO Object is null");
            
            var createdTweet = await _service.TweetService.CreateTweet(UserId,tweet,trackChanges:true);
            
            return Ok(createdTweet);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTweet(int id)
        {
            await _service.TweetService.DeleteTweet(id, trackChanges: false);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult>UpdateTweet(TweetForUpdateDTO tweetForUpdateDTO)
        {
            var tweet=await _service.TweetService.UpdateTweet(tweetForUpdateDTO);   
            return Ok(tweet);

        }
        // Likes Creation,Retreival and Removal
     

        [HttpPost("like/{userId:int}/{tweetId:int}")]
        public async Task<IActionResult> AddLike(int userId,int tweetId)
        {
            await _service.LikeService.CreateLike(userId, tweetId,trackChanges:false);

            return Ok();
        }
        [HttpDelete("like/{userId:int}/{tweetId:int}")]
        public async Task<IActionResult> RemoveLike(int userId, int tweetId)
        {
            await _service.LikeService.DeleteLike(userId, tweetId, trackChanges: false);

            return Ok();
        }
        // Retweets Creation, Retreival and Removal

       
        [HttpPost("retweet/{userId:int}/{tweetId:int}")]
        public async Task<IActionResult> AddRetweet(int userId, int tweetId)
        {
            await _service.RetweetService.CreateRetweet(userId, tweetId, trackChanges: false);

            return Ok();
        }
        [HttpDelete("retweet/{userId:int}/{tweetId:int}")]
        public async Task<IActionResult> RemoveRetweet(int userId, int tweetId)
        {
            await _service.RetweetService.DeleteRetweet(userId, tweetId, trackChanges: false);

            return Ok();
        }


    }
}
