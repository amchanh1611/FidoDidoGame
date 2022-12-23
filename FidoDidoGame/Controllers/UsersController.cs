using FidoDidoGame.Modules.Users.Request;
using FidoDidoGame.Modules.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace FidoDidoGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;

        public UsersController(IUserService service)
        {
            this.service = service;
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequest request)
        {
            service.Create(request);
            return Ok();
        }
        [HttpPost("Status")]
        public IActionResult CreateStatus([FromBody] List<string> statusNames)
        {
            service.CreateStatus(statusNames);
            return Ok();
        }
        [HttpPut("{userId}")]
        public IActionResult Update([FromRoute] int userId, [FromBody] UpdateUserRequest request)
        {
            service.Update(userId, request);
            return Ok();
        }
        [HttpGet("Profile/{userId}")]
        public IActionResult Profile([FromRoute] int userId, [FromQuery] ProfilesRequest request)
        {
            return Ok(service.Profile(userId));
        }
    }
}
