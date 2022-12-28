namespace FidoDidoGame.Persistents.Redis.Entities
{
    public class UserRankOfDayIn
    {
        public DateTime Date { get; set; }
        public string UserName { get; set; } = default!;

        public UserRankOfDayIn(DateTime date, string userName)
        {
            Date = date;
            UserName = userName;
        }
    }
}
