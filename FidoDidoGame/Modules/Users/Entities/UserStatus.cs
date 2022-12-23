namespace FidoDidoGame.Modules.Users.Entities
{
    public class UserStatus
    {
        public int UserId { get; set; }
        public string StatusCode { get; set; } = default!;
        public User? User { get; set; }
        public Status? Status { get; set; }
    }
}
