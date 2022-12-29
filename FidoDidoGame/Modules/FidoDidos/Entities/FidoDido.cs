using System.Text.Json.Serialization;

namespace FidoDidoGame.Modules.FidoDidos.Entities
{
    public class FidoDido
    {
        public int FidoId { get; set; }
        public int DidoId { get; set; }
        public int Percent { get; set; }
        public int PercentRand { get; set; }
        public SpecialStatus SpecialStatus { get; set; }
        public int Point { get; set; } = default!;
        public Fido? Fido { get; set; }
        public Dido? Dido { get; set; }
    }

    public enum SpecialStatus
    {
        Normal = 1,
        X2,
        Auto,
        Heal,
        Ban,
        Point
    }
}
