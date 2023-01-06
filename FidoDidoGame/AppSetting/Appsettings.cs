namespace FidoDidoGame.AppSetting;

public class AppSettings
{
    public Jwt? Jwt { get; set; }
}
public class Jwt
{
    public string? Issuer { get; set; }
    public string? AccessKey { get; set; }
    public string? RefreshKey { get; set; }
}
