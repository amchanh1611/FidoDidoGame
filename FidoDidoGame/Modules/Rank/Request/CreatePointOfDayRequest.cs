namespace FidoDidoGame.Modules.Rank.Request
{
    public class CreatePointOfDayRequest : CreateOrUpdatePointOfDay
    {
        public long UserId { get; set; }
    }
    public class UpdatePointOfDayRequest : CreateOrUpdatePointOfDay
    {

    }
    public class CreateOrUpdatePointOfDay
    {
        public string? UserName { get; set; }
        public int Point { get; set; }
        public DateTime Date { get; set; }
    }
}
