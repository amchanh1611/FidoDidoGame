namespace FidoDidoGame.Persistents.Redis.Entities
{
    public class UserRankOfDayIn
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = default!;

        public UserRankOfDayIn(DateTime date, int userId, string name)
        {
            Date = date;
            UserId = userId;
            Name = name;
        }
        public UserRankOfDayIn() { }
    }
}
