namespace FidoDidoGame.Modules.Rank.Request
{
    public class UpdateRank
    {
        public string? UserName { get; set; }

        public long UserId { get; set; }

        public int Point { get; set; }

        public DateTime Date { get; set; }

        public UpdateRank(string? userName, long userId, int point, DateTime date)
        {
            UserName = userName;
            UserId = userId;
            Point = point;
            Date = date;
        }

    }
}
