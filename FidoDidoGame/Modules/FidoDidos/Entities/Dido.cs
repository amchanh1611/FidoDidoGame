namespace FidoDidoGame.Modules.FidoDidos.Entities
{
    public class Dido
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<FidoDido> FidoDidos { get; set; } = default!;
    }
}
