using FidoDidoGame.Modules.FidoDidos.Entities;

namespace FidoDidoGame.Modules.FidoDidos.Response
{
    public class DidoResponse
    {
        public string? DidoName { get; set; }
        public SpecialStatus SpecialStatus { get; set; }
        public int Point { get; set; }
        public DidoResponse(string didoName, SpecialStatus specialStatus, int point)
        {
            DidoName = didoName;
            Point = point;
            SpecialStatus = specialStatus;
        }
    }
}
