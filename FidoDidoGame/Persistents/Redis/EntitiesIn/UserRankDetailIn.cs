using System.Text.Json.Serialization;

namespace FidoDidoGame.Persistents.Redis.Entities
{
    public class UserRankDetailIn
    {
        public DateTime Date { get; set; }
        public string UserName { get; set; } = default!;
        public string Point { get; set; }
        public string IsX2 { get; set; }
        public UserRankDetailIn(DateTime date, string name, string point, string isX2)
        {
            Date = date;
            UserName = name;
            Point = point;
            IsX2 = isX2;
        }
    }
}
