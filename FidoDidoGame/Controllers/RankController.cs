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

        [HttpGet("History"), Authorize]
        public IActionResult HistoryOf([FromQuery] HistoryOfRequest request)
        {
            int userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(service.HistoryOf(userId, request));
        }

        [HttpGet("UserRank"), Authorize]
        public IActionResult UserRank([FromQuery] GetUserRankRequest request)
        {
            int userId = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(service.UserRank(userId,request));
        }

        [HttpPost("CreateEvent")]
        public IActionResult CreateEvent([FromBody] CreateEventRequest request)
        {
            service.CreateEvent(request);
            return Ok();
        }
        [HttpPost("UpdateEvent/{eventId}")]
        public IActionResult UpdateEvent([FromRoute] int eventId, [FromBody] UpdateEventRequest request)
        {
            service.UpdateEvent(eventId, request);
            return Ok();
        }
    }
}
