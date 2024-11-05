using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace Kwikker_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IServiceManager _service;
        public NotificationsController(IServiceManager service) => _service = service;
        [HttpGet("user/{receiverId:int}")]
        public async Task<IActionResult>GetUserNotifications(int receiverId)
        {
           var notifications= await _service.NotificationService.GetUserNotificationsAsync(receiverId,trackChanges:false);
            return Ok(notifications);
        }
       

    }
}
