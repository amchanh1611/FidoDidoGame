namespace FidoDidoGame.Modules.Users.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<UserStatus>? UserStatus { get; set; }
    }
}
