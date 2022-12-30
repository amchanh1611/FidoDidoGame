namespace FidoDidoGame.Persistents.Redis.Entities
{
    public class UserRankOfDayIn
    {
        public long DateMiliSecond { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = default!;
        public int Point { get; set; }
        public UserRankOfDayIn(long dateMiliSecond, string userName, int point, int userId)
        {
            DateMiliSecond = dateMiliSecond;
            UserName = userName;
            Point = point;
            UserId = userId;
        }
    }
}
