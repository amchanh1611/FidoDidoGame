using System.Text.Json.Serialization;

namespace FidoDidoGame.Modules.Rank.Response
{
    public class RankingResponse
    {
        public int Point { get; set; }
        public string? UserName { get; set; }
        [JsonIgnore]
        public DateTime Date { get; set; }
    }
}
