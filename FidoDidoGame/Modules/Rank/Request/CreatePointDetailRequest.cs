using FidoDidoGame.Modules.FidoDidos.Entities;

namespace FidoDidoGame.Modules.Rank.Request
{
    public class CreatePointDetailRequest
    {
        public long UserId { get; set; }

        public string? UserName { get; set; }

        public SpecialStatus SpecialStatus { get; set; }

        public int? Point { get; set; }

        public DateTime Date { get; set; }

        public int? IsX2 { get; set; }

        public CreatePointDetailRequest(long userId, string? userName, int? point, DateTime date, int? isX2, SpecialStatus specialStatus)
        {
            UserId = userId;
            UserName = userName;
            Point = point;
            Date = date;
            IsX2 = isX2;
            SpecialStatus = specialStatus;
        }
    }
}
