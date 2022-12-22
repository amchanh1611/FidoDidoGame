using FidoDidoGame.Modules.Users.Entities;

namespace FidoDidoGame.Modules.FidoDidos.Entities
{
    public class Fido
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<User> Users { get; set; } = default!;
        public ICollection<FidoDido> FidoDidos { get; set; } = default!;
    }
}
