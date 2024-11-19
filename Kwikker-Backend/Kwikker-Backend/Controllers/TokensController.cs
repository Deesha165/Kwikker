using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

namespace Kwikker_Backend.Controllers
{
    [Route("api/[controller]")]
 
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IServiceManager _service;
        public TokensController(IServiceManager service) => _service = service;
        [HttpPost("refresh")]
       
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await
            _service.AuthenticationService.RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }
    }
}
