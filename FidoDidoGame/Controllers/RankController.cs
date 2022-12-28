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
        [HttpGet]
        public IActionResult Ranking()
        {
            service.Ranking();
            return Ok();
        }
    }
}
