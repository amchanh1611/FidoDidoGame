﻿// <auto-generated />
using System;
using FidoDidoGame.Persistents.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FidoDidoGame.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FidoDidoGame.Modules.FidoDidos.Entities.Dido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("dido", (string)null);
                });

            modelBuilder.Entity("FidoDidoGame.Modules.FidoDidos.Entities.Fido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Percent")
                        .HasColumnType("int");

                    b.Property<int>("PercentRand")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("fido", (string)null);
                });

            modelBuilder.Entity("FidoDidoGame.Modules.FidoDidos.Entities.FidoDido", b =>
                {
                    b.Property<int>("FidoId")
                        .HasColumnType("int");

                    b.Property<int>("DidoId")
                        .HasColumnType("int");

                    b.Property<int>("Percent")
                        .HasColumnType("int");

                    b.Property<int>("PercentRand")
                        .HasColumnType("int");

                    b.Property<string>("Point")
                        .IsRequired()
                        .HasColumnType("char(9)");

                    b.HasKey("FidoId", "DidoId");

                    b.HasIndex("DidoId");

                    b.ToTable("fido_dido", (string)null);
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Ranks.Entities.PointDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("point_detail", (string)null);
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Ranks.Entities.PointOfDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("point_of_day", (string)null);
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Users.Entities.Status", b =>
                {
                    b.Property<string>("StatusCode")
                        .HasColumnType("char(9)");

                    b.HasKey("StatusCode");

                    b.ToTable("status", (string)null);
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Users.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<int?>("FidoId")
                        .HasColumnType("int");

                    b.Property<sbyte?>("Male")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("char(12)");

                    b.HasKey("Id");

                    b.HasIndex("FidoId");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Users.Entities.UserStatus", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("StatusCode")
                        .HasColumnType("char(9)");

                    b.HasKey("UserId", "StatusCode");

                    b.HasIndex("StatusCode");

                    b.ToTable("user_status", (string)null);
                });

            modelBuilder.Entity("FidoDidoGame.Modules.FidoDidos.Entities.FidoDido", b =>
                {
                    b.HasOne("FidoDidoGame.Modules.FidoDidos.Entities.Dido", "Dido")
                        .WithMany("FidoDidos")
                        .HasForeignKey("DidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FidoDidoGame.Modules.FidoDidos.Entities.Fido", "Fido")
                        .WithMany("FidoDidos")
                        .HasForeignKey("FidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dido");

                    b.Navigation("Fido");
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Ranks.Entities.PointDetail", b =>
                {
                    b.HasOne("FidoDidoGame.Modules.Users.Entities.User", "User")
                        .WithMany("PointDetails")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Ranks.Entities.PointOfDay", b =>
                {
                    b.HasOne("FidoDidoGame.Modules.Users.Entities.User", "User")
                        .WithMany("PointOfDays")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Users.Entities.User", b =>
                {
                    b.HasOne("FidoDidoGame.Modules.FidoDidos.Entities.Fido", "Fido")
                        .WithMany("Users")
                        .HasForeignKey("FidoId");

                    b.Navigation("Fido");
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Users.Entities.UserStatus", b =>
                {
                    b.HasOne("FidoDidoGame.Modules.Users.Entities.Status", "Status")
                        .WithMany("UserStatus")
                        .HasForeignKey("StatusCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FidoDidoGame.Modules.Users.Entities.User", "User")
                        .WithMany("UserStatus")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FidoDidoGame.Modules.FidoDidos.Entities.Dido", b =>
                {
                    b.Navigation("FidoDidos");
                });

            modelBuilder.Entity("FidoDidoGame.Modules.FidoDidos.Entities.Fido", b =>
                {
                    b.Navigation("FidoDidos");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Users.Entities.Status", b =>
                {
                    b.Navigation("UserStatus");
                });

            modelBuilder.Entity("FidoDidoGame.Modules.Users.Entities.User", b =>
                {
                    b.Navigation("PointDetails");

                    b.Navigation("PointOfDays");

                    b.Navigation("UserStatus");
                });
#pragma warning restore 612, 618
        }
    }
}
