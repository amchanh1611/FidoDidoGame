using System.Text.Json.Serialization;

namespace FidoDidoGame.Persistents.Redis.Entities
{
    public class UserRankDetailIn
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = default!;
        public string Point { get; set; }
        public UserRankDetailIn(DateTime date, int userId, string name, string point)
        {
            Date = date;
            UserId = userId;
            Name = name;
            Point = point;
        }
    }
}
