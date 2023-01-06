namespace FidoDidoGame.Modules.Users.Entities
{
    public class Reward
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public Award Award { get; set; } = default!;
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public User? User { get; set; }
        public Reward(long userId, Award award, DateTime dateStart, DateTime dateEnd)
        {
            UserId = userId;
            Award = award;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
    public enum Award
    {
        First = 1,
        Second,
        Third,
        Consolation
    }
}
