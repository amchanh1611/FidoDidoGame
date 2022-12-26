namespace FidoDidoGame.Modules.FidoDidos.Response
{
    public class DidoResponse
    {
        public string? DidoName { get; set; }
        public string? Point { get; set; }
        public DidoResponse(string didoName, string point)
        {
            DidoName = didoName;
            Point = point;
        }
    }
}
