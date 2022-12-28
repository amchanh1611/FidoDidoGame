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
    }
}
