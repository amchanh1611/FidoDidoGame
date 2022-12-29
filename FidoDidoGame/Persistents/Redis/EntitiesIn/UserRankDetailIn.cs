using FidoDidoGame.Modules.FidoDidos.Entities;
using System.Text.Json.Serialization;

namespace FidoDidoGame.Persistents.Redis.Entities;

public class UserRankDetailIn
{
    public DateTime Date { get; set; }
    public string UserName { get; set; } = default!;
    public SpecialStatus SpecialStatus { get; set; }
    public int Point { get; set; }
    public int IsX2 { get; set; }
    public UserRankDetailIn(DateTime date, string userName, int point, int isX2, SpecialStatus specialStatus)
    {
        Date = date;
        UserName = userName;
        Point = point;
        IsX2 = isX2;
        SpecialStatus = specialStatus;
    }
}
