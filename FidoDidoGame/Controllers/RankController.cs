using FidoDidoGame.Modules.Rank.Request;
using FidoDidoGame.Modules.Ranks.Request;
using FidoDidoGame.Modules.Ranks.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FidoDidoGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private readonly IRankService service;

        public RankController(IRankService service)
        {
            this.service = service;
        }

        [HttpGet("Rank"), Authorize]
        public IActionResult Ranking([FromQuery] GetRankRequest request)
        {
            return Ok(service.Ranking(request));
        }

        [HttpGet("HistoryOf"), Authorize]
        public IActionResult HistoryOf([FromQuery] HistoryOfRequest request)
        {
            long userId = long.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(service.HistoryOf(userId, request));
        }

        [HttpGet("UserRank"), Authorize]
        public IActionResult UserRank([FromQuery] GetUserRankRequest request)
        {
            long userId = long.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(service.UserRank(userId,request));
        }

        [HttpPost("Event")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateEvent([FromBody] CreateEventRequest request)
        {
            service.CreateEvent(request);
            return Ok();
        }

        [HttpPut("Event/{eventId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateEvent([FromRoute] int eventId, [FromBody] UpdateEventRequest request)
        {
            service.UpdateEvent(eventId, request);
            return Ok();
        }
    }
}
