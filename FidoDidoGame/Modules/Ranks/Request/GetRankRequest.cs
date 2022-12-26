namespace FidoDidoGame.Modules.Ranks.Request
{
    public class GetRankRequest
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Current { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
