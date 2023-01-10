using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Users.Entities;

namespace FidoDidoGame.Modules.FidoDidos.Response
{
    public class FidoResponse
    {
        public FidoResponse(long? userId, string fido, List<SpecialStatus>? status)
        {
            UserId = userId;
            Status = status;
            Fido = fido;
        }

        public long? UserId { get; set; }
        public string? Fido { get; set; }
        public List<SpecialStatus>? Status { get; set; }
    }
}
