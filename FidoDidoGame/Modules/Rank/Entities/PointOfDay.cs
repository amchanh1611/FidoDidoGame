using FidoDidoGame.Modules.Users.Entities;

namespace FidoDidoGame.Modules.Ranks.Entities
{
    public class PointOfDay
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int Point { get; set; }
        public DateTime Date { get; set; }
        public User? User { get; set; }
    }
}
