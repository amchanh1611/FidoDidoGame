using FidoDidoGame.Modules.FidoDidos.Entities;
using FidoDidoGame.Modules.Ranks.Entities;
using FidoDidoGame.Modules.Users.Entities;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace FidoDidoGame.Persistents.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.ToTable(nameof(User).Underscore());
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).HasColumnType("text");
            builder.Property(x => x.NickName).HasColumnType("text");
            builder.Property(x => x.Phone).HasColumnType("char(12)");
            builder.Property(x => x.Address).HasColumnType("text");
            builder.Property(x => x.Male).HasColumnType("tinyint");
            builder.Property(x => x.IdCard).HasColumnType("char(15)");
            builder.Property(x => x.Avatar).HasColumnType("text");
            builder.Property(x => x.RefreshToken).HasColumnType("text");
            builder.HasOne(x => x.Fido)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.FidoId);
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
            builder.Property(x => x.SpecialStatus).HasColumnType("tinyint");
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
            builder.Property(x => x.Point).HasColumnType("char(9)");
            builder.Property(x => x.IsX2).HasColumnType("char(9)");
            builder.HasOne(x => x.User)
            .WithMany(x => x.PointDetails)
            .HasForeignKey(x => x.UserId);
        });
        modelBuilder.Entity<Reward>(builder =>
        {
            builder.ToTable(nameof(Reward).Underscore());
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Award).HasColumnType("tinyint");
            builder.HasOne(x => x.User)
            .WithMany(x => x.Rewards)
            .HasForeignKey(x => x.UserId);
        });
    }
}
