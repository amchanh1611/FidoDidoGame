namespace FidoDidoGame.Persistents.Redis.Entities
{
    public class UserRankOfDayIn
    {
        public long DateMiliSecond { get; set; }
        public string UserName { get; set; } = default!;
        public int Point { get; set; }
        public DateTime Date { get; set; }
        public UserRankOfDayIn(long dateMiliSecond, string userName, int point, DateTime date)
        {
            DateMiliSecond = dateMiliSecond;
            UserName = userName;
            Point = point;
            Date = date;
        }
    }
}
