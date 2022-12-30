using FidoDidoGame.Persistents.Redis.Entities;
using System.Text.Json.Serialization;

namespace FidoDidoGame.Modules.Rank.Response
{
    public class RankingResponse
    {
        public int Point { get; set; }
        public string? UserName { get; set; }
        public DateTime Date { get; set; }
    }
    public class UserRankReponse
    {
        public long? Rank { get; set; }
        public RankingResponse? User { get; set; }
        public UserRankReponse(long? rank, RankingResponse? user)
        {
            Rank = rank;
            User = user;
        }
    }
}
