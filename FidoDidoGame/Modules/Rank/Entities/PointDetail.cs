using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Users.Entities;

namespace FidoDidoGame.Modules.Ranks.Entities
{
    public class PointDetail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public SpecialStatus SpecialStatus { get; set; }
        public int Point { get; set; } = default!;
        public DateTime Date { get; set; }
        public int IsX2 { get; set; } = default!;
        public User? User { get; set; }

        public PointDetail(int userId, int point, DateTime date, int isX2, SpecialStatus specialStatus)
        {
            UserId = userId;
            Point = point;
            Date = date;
            IsX2 = isX2;
            SpecialStatus = specialStatus;
        }
        public PointDetail() { }
    }
}
