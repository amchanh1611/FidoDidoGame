using FidoDidoGame.Modules.Users.Entities;

namespace FidoDidoGame.Modules.Ranks.Entities
{
    public class PointDetail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Point { get; set; } = default!;
        public DateTime Date { get; set; }
        public User? User { get; set; }
    }
}
