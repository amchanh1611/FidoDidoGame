using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Ranks.Entities;
using FidoDidoGame.Modules.Users.Entities;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace FidoDidoGame.Persistents.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable(nameof(User).Underscore());
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Name).HasColumnType("text");
                builder.Property(x => x.NickName).HasColumnType("text");
                builder.Property(x => x.Phone).HasColumnType("char(12)");
                builder.Property(x => x.Address).HasColumnType("text");
                builder.Property(x => x.Male).HasColumnType("tinyint");
                builder.Property(x => x.Avatar).HasColumnType("text");
                builder.HasOne(x => x.Fido)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.FidoId);
            });
            modelBuilder.Entity<Status>(builder =>
            {
                builder.ToTable(nameof(Status).Underscore());
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Name).HasColumnType("text");

            });
            modelBuilder.Entity<UserStatus>(builder =>
            {
                builder.ToTable(nameof(UserStatus).Underscore());
                builder.HasKey(x => new { x.UserId, x.StatusId });
                builder.HasOne(x => x.User)
                .WithMany(x => x.UserStatus)
                .HasForeignKey(x => x.UserId);
                builder.HasOne(x => x.Status)
                .WithMany(x => x.UserStatus)
                .HasForeignKey(x => x.StatusId);
            });
            modelBuilder.Entity<Fido>(builder =>
            {
                builder.ToTable(nameof(Fido).Underscore());
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Name).HasColumnType("text");
            });
            modelBuilder.Entity<Dido>(builder =>
            {
                builder.ToTable(nameof(Dido).Underscore());
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Name).HasColumnType("text");
            });
            modelBuilder.Entity<FidoDido>(builder =>
            {
                builder.ToTable(nameof(FidoDido).Underscore());
                builder.HasKey(x => new { x.FidoId, x.DidoId });
                builder.Property(x => x.Point).HasColumnType("char(9)");
            });
            modelBuilder.Entity<PointOfDay>(builder =>
            {
                builder.ToTable(nameof(PointOfDay).Underscore());
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.User)
                .WithMany(x => x.PointOfDays)
                .HasForeignKey(x => x.UserId);
            });
            modelBuilder.Entity<PointDetail>(builder =>
            {
                builder.ToTable(nameof(PointDetail).Underscore());
                builder.HasKey(x => x.Id);
                builder.HasOne(x => x.User)
                .WithMany(x => x.PointDetails)
                .HasForeignKey(x => x.UserId);
            });
        }
    }
}
