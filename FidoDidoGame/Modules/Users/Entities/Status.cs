namespace FidoDidoGame.Modules.Users.Entities
{
    public class Status
    {
        public string StatusCode { get; set; } = default!;
        public ICollection<UserStatus>? UserStatus { get; set; }
    }
}
