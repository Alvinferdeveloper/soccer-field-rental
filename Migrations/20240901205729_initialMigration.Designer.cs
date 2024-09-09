﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using proyectoCanchas.Models;

#nullable disable

namespace proyectoCanchas.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240901205729_initialMigration")]
    partial class initialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("proyectoCanchas.Models.Authoritie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Authorities");
                });

            modelBuilder.Entity("proyectoCanchas.Models.Field", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("ServiceEndTime")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("ServiceStartTime")
                        .HasColumnType("time(6)");

                    b.Property<string>("SoccerType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("proyectoCanchas.Models.Rent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClientCedula")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ClientLastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ClientType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Datetime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FieldId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("UserId");

                    b.ToTable("Rents");
                });

            modelBuilder.Entity("proyectoCanchas.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("proyectoCanchas.Models.RoleAuthoritie", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("AuthoritieId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "AuthoritieId");

                    b.HasIndex("AuthoritieId");

                    b.ToTable("RoleAuthorities");
                });

            modelBuilder.Entity("proyectoCanchas.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Birthday")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("proyectoCanchas.Models.Rent", b =>
                {
                    b.HasOne("proyectoCanchas.Models.Field", "Field")
                        .WithMany("Rents")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("proyectoCanchas.Models.User", "User")
                        .WithMany("Rents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("User");
                });

            modelBuilder.Entity("proyectoCanchas.Models.RoleAuthoritie", b =>
                {
                    b.HasOne("proyectoCanchas.Models.Authoritie", "Authoritie")
                        .WithMany("RoleAuthorities")
                        .HasForeignKey("AuthoritieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("proyectoCanchas.Models.Role", "Role")
                        .WithMany("RoleAuthorities")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Authoritie");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("proyectoCanchas.Models.User", b =>
                {
                    b.HasOne("proyectoCanchas.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("proyectoCanchas.Models.Authoritie", b =>
                {
                    b.Navigation("RoleAuthorities");
                });

            modelBuilder.Entity("proyectoCanchas.Models.Field", b =>
                {
                    b.Navigation("Rents");
                });

            modelBuilder.Entity("proyectoCanchas.Models.Role", b =>
                {
                    b.Navigation("RoleAuthorities");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("proyectoCanchas.Models.User", b =>
                {
                    b.Navigation("Rents");
                });
#pragma warning restore 612, 618
        }
    }
}
