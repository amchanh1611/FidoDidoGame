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
        public IActionResult CreateFido([FromBody] List<string> fidoNames)
        {
            service.CreateMultiFido(fidoNames);
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
    }
}
