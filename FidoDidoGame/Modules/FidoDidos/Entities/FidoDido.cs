namespace FidoDidoGame.Modules.FidoDidos.Entities
{
    public class FidoDido
    {
        public int FidoId { get; set; }
        public int DidoId { get; set; }
        public int Percent { get; set; }
        public string Point { get; set; } = default!;
        public Fido? Fido { get; set; }
        public Dido? Dido { get; set; }
    }
}
