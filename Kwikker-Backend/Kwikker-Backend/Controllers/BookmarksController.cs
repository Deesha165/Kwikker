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
    public class BookmarksController : ControllerBase
    {
        private readonly IServiceManager _service;
        public BookmarksController(IServiceManager service) => _service = service;

        //Bookmarks Creation, Retreival and Removal
        [HttpPost("{UserId:int}/{TweetId:int}")]
        public async Task<IActionResult> CreateBookmark(int UserId,int TweetId)
        {
        await _service.BookmarkService.CreateBookmark(UserId, TweetId,trackChanges:false);

            return Ok();
        }



        [HttpDelete("{UserId:int}/{TweetId:int}")]
        public async Task<IActionResult> RemoveBookmark(int UserId, int TweetId)
        {
            await _service.BookmarkService.DeleteBookmark(UserId, TweetId,trackChanges:false);
            return Ok();
        }

        [HttpGet("{UserId:int}")]
        public async Task<IActionResult> GetBookmarksByUser(int UserId)
        {
           var Bookmarks= await _service.BookmarkService.GetUserBookmarks(UserId,trackChanges:false);

            return Ok(Bookmarks);
        }

    }


}
