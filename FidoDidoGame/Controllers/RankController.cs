using FidoDidoGame.Modules.Ranks.Request;
using FidoDidoGame.Modules.Ranks.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("Rank")]
        public IActionResult Ranking([FromQuery] GetRankRequest request)
        {
            return Ok(service.Ranking(request));
        }
        [HttpGet("History/User/{userId}")]
        public IActionResult HistoryOf([FromRoute] int userId, [FromQuery] HistoryOfRequest request)
        {
            return Ok(service.HistoryOf(userId, request));
        }
        [HttpGet("UserRank/{userId}")]
        public IActionResult UserRank([FromRoute] int userId, [FromQuery] GetUserRankRequest request)
        {
            return Ok(service.UserRank(userId,request));
        }
    }
}
