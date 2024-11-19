using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace Kwikker_Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IServiceManager _service;
        public UsersController(IServiceManager service) => _service = service;
        [HttpGet("{id:int}",Name ="UserById")]
        public async Task<IActionResult> GetUser(int id)
        {
            

            var user = await _service.UserService.GetUser(id,trackChanges: false);
                
            return Ok(user);
            

        }
    }
}
