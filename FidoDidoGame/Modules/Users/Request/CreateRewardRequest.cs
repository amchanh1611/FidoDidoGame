using FidoDidoGame.Modules.Users.Entities;

namespace FidoDidoGame.Modules.Users.Request
{
    public class CreateRewardRequest
    {
        public long UserId { get; set; }
        public Award Award { get; set; } = default!;
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public CreateRewardRequest(long userId, Award award, DateTime dateStart, DateTime dateEnd)
        {
            UserId = userId;
            Award = award;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
}
