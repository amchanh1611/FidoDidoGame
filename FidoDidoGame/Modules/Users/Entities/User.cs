using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Ranks.Entities;
using System.Text.Json.Serialization;

namespace FidoDidoGame.Modules.Users.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string NickName { get; set; } = default!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public UserMale? Male { get; set; }
        public string? Avatar { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int? FidoId { get; set; }
        public Fido? Fido { get; set; }
        public ICollection<PointOfDay>? PointOfDays { get; set; }
        public ICollection<PointDetail>? PointDetails { get; set; }
    }
    public enum UserMale
    {
        Male = 1,
        Female
    }
    public enum UserStatus
    {
        Normal = 1,
        X2,
        Auto,
        Heal,
        Ban
    }
}
