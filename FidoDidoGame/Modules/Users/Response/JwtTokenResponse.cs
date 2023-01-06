namespace FidoDidoGame.Modules.Users.Response
{
    public class JwtTokenResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
