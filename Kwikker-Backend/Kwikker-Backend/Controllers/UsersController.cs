using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Kwikker_Backend.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IServiceManager _service;
        public UsersController(IServiceManager service) => _service = service;
       
        [HttpGet("{id:int}",Name ="UserById")]
        public async Task<IActionResult> GetUser(int id,[FromQuery]UserParameters userParameters)
       {
            

           var user = await _service.UserService.GetUser(id,trackChanges: false,userParameters);
                
            return Ok(user);
            

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserForUpdateDTO userForUpdateDTO)
        {
            var tweet = await _service.UserService.UpdateUser(userForUpdateDTO);
            return Ok(tweet);

        }
    }
}
