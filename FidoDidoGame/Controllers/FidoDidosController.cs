using FidoDidoGame.Modules.FidoDidos.Request;
using FidoDidoGame.Modules.FidoDidos.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FidoDidoGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FidoDidosController : ControllerBase
    {
        private readonly IFidoDidoService service;

        public FidoDidosController(IFidoDidoService service)
        {
            this.service = service;
        }

        [HttpPost("Fido")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateFido([FromBody] CreateFidoRequest request)
        {
            service.CreateFido(request);
            return Ok();
        }

        [HttpPost("Dido")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateDido([FromBody] List<string> didoNames)
        {
            service.CreateMultiDido(didoNames);
            return Ok();
        }

        [HttpPost("FidoDido")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateFidoDido([FromBody] CreateFidoDidoRequest request)
        {
            service.CreateFidoDido(request);
            return Ok();
        }
        [HttpGet("Fido"), Authorize]
        public IActionResult Fido()
        {
            long userId = long.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(service.Fido(userId));
        }

        [HttpGet("Dido"), Authorize]
        public IActionResult Dido()
        {
            long userId = long.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(service.Dido(userId));
        }

        [HttpPut("Fido/{fidoId}/Percent")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateFidoPercent([FromRoute] int fidoId, UpdateFidoPercentRequest request)
        {
            service.UpdateFidoPercent(fidoId, request);
            return Ok();
        }
    }
}
