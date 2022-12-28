namespace FidoDidoGame.Modules.Rank.Request
{
    public class CreatePointDetailRequest
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Point { get; set; }
        public DateTime Date { get; set; }
        public string? IsX2 { get; set; }
    }
}
