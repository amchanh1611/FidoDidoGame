using FidoDidoGame.Modules.Ranks.Entities;

namespace FidoDidoGame.Modules.Rank.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public int Round { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public ICollection<PointOfRound> PointOfRounds { get; set; } = default!;
    }
}
