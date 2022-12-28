namespace FidoDidoGame.Modules.Rank.Request
{
    public class UpdateRank
    {
        public string? UserName { get; set; }
        public int UserId { get; set; }
        public int Point { get; set; }
        public DateTime Date { get; set; }
    }
}
