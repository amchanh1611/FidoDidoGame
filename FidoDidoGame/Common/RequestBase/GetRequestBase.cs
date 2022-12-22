namespace FidoDidoGame.Common.RequestBase
{
    public class GetRequestBase
    {
        public string? InfoSearch { get; set; }
        public int Current { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Orderby { get; set; }
    }
}
