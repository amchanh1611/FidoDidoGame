namespace FidoDidoGame.Modules.Rank.Request
{
    public class CreatePointOfRoundRequest : CreateOrUpdatePointOfRound
    {
        public long UserId { get; set; }

        public CreatePointOfRoundRequest(long userId, string? userName, int point, DateTime date, int eventId) : base(userName, point, date, eventId)
        {
            UserId = userId;
        }
    }
    public class UpdatePointOfRoundRequest : CreateOrUpdatePointOfRound
    {

    }
    public class CreateOrUpdatePointOfRound
    {
        public string? UserName { get; set; }
        public int Point { get; set; }
        public DateTime Date { get; set; }
        public int EventId { get; set; }

        public CreateOrUpdatePointOfRound(string? userName, int point, DateTime date, int eventId)
        {
            UserName = userName;
            Point = point;
            Date = date;
            EventId = eventId;
        }
        public CreateOrUpdatePointOfRound()
        {

        }
    }

}
