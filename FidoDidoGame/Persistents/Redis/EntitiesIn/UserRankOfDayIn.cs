namespace FidoDidoGame.Persistents.Redis.Entities
{
    public class UserRankOfDayIn
    {
        public long DateMiliSecond { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = default!;
        public int Point { get; set; }
        public DateTime Date { get; set; }
        public UserRankOfDayIn(long dateMiliSecond, string userName, int point, long userId, DateTime date)
        {
            DateMiliSecond = dateMiliSecond;
            UserName = userName;
            Point = point;
            UserId = userId;
            Date = date;
        }
    }
}
