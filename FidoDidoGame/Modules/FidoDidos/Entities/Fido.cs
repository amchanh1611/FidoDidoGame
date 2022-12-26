using FidoDidoGame.Modules.Users.Entities;
using System.Text.Json.Serialization;

namespace FidoDidoGame.Modules.FidoDidos.Entities
{
    public class Fido
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int Percent { get; set; }
        public int PercentRand { get; set; }
        public ICollection<User> Users { get; set; } = default!;
        public ICollection<FidoDido> FidoDidos { get; set; } = default!;
    }
}
