using FidoDidoGame.Modules.Users.Entities;

namespace FidoDidoGame.Modules.Ranks.Entities
{
    public class PointDetail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Point { get; set; } = default!;
        public DateTime Date { get; set; }
        public string IsX2 { get; set; } = default!;
        public User? User { get; set; }

        public PointDetail(int userId, string point, DateTime date, string isX2)
        {
            UserId = userId;
            Point = point;
            Date = date;
            IsX2 = isX2;
        }
        public PointDetail() { }
    }
}
