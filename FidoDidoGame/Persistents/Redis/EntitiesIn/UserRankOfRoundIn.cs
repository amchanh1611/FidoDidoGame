namespace FidoDidoGame.Persistents.Redis.Entities
{
    public class UserRankOfRoundIn
    {
        public long DateMiliSecond { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = default!;
        public int Point { get; set; }
        public UserRankOfRoundIn(long dateMiliSecond, string userName, int point, long userId)
        {
            DateMiliSecond = dateMiliSecond;
            UserName = userName;
            Point = point;
            UserId = userId;
        }
    }
}
