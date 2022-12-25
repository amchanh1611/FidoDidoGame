using FidoDidoGame.Modules.FidoDidos.Request;
using FidoDidoGame.Modules.FidoDidos.Service;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateFido([FromBody] CreateFidoRequest request)
        {
            service.CreateFido(request);
            return Ok();
        }
        [HttpPost("Dido")]
        public IActionResult CreateDido([FromBody] List<string> didoNames)
        {
            service.CreateMultiDido(didoNames);
            return Ok();
        }
        [HttpPost("FidoDido")]
        public IActionResult CreateFidoDido([FromBody] CreateFidoDidoRequest request)
        {
            service.CreateFidoDido(request);
            return Ok();
        }
        [HttpGet("Fido/{userId}")]
        public IActionResult Fido([FromRoute] int userId)
        {
            return Ok(service.Fido(userId));
        }

        [HttpGet("Dido/{userId}")]
        public IActionResult Dido([FromRoute] int userId)
        {
            return Ok(service.Dido(userId));
        }

        [HttpPut("Fido/{fidoId}/Percent")]
        public IActionResult UpdateFidoPercent([FromRoute] int fidoId, UpdateFidoPercentRequest request)
        {
            service.UpdateFidoPercent(fidoId, request);
            return Ok();
        }
    }
}
