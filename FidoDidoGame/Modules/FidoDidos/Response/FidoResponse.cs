namespace FidoDidoGame.Modules.FidoDidos.Response
{
    public class FidoResponse
    {
        public FidoResponse(int? userId, string fido, List<string>? status)
        {
            UserId = userId;
            Status = status;
            Fido = fido;
        }

        public int? UserId { get; set; }
        public string? Fido { get; set; }
        public List<string>? Status { get; set; }
    }
}
