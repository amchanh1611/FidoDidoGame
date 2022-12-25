using FidoDidoGame.Modules.Users.Entities;

namespace FidoDidoGame.Modules.FidoDidos.Response
{
    public class FidoResponse
    {
        public FidoResponse(int? userId, string fido, List<UserStatus>? status)
        {
            UserId = userId;
            Status = status;
            Fido = fido;
        }

        public int? UserId { get; set; }
        public string? Fido { get; set; }
        public List<UserStatus>? Status { get; set; }
    }
}
