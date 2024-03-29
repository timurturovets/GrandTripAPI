﻿// <auto-generated />
using System;
using GrandTripAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GrandTripAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20221205215341_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GrandTripAPI.Models.Dot", b =>
                {
                    b.Property<int>("InstanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DotDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DotId")
                        .HasColumnType("int");

                    b.Property<string>("DotName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PositionX")
                        .HasColumnType("float");

                    b.Property<double>("PositionY")
                        .HasColumnType("float");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.HasKey("InstanceId");

                    b.HasIndex("RouteId");

                    b.ToTable("Dots");
                });

            modelBuilder.Entity("GrandTripAPI.Models.Line", b =>
                {
                    b.Property<int>("InstanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LatLngs")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LineId")
                        .HasColumnType("int");

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.HasKey("InstanceId");

                    b.HasIndex("RouteId");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("GrandTripAPI.Models.Route", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RouteName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int?>("ThemeId")
                        .HasColumnType("int");

                    b.HasKey("RouteId");

                    b.HasIndex("CreatorId");

                    b.HasIndex("SeasonId");

                    b.HasIndex("ThemeId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("GrandTripAPI.Models.RouteSeason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Seasons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Все сезоны"
                        });
                });

            modelBuilder.Entity("GrandTripAPI.Models.RouteTheme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Themes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Без тематики"
                        });
                });

            modelBuilder.Entity("GrandTripAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GrandTripAPI.Models.Dot", b =>
                {
                    b.HasOne("GrandTripAPI.Models.Route", "Route")
                        .WithMany("Dots")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Route");
                });

            modelBuilder.Entity("GrandTripAPI.Models.Line", b =>
                {
                    b.HasOne("GrandTripAPI.Models.Route", null)
                        .WithMany("Lines")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GrandTripAPI.Models.Route", b =>
                {
                    b.HasOne("GrandTripAPI.Models.User", "Creator")
                        .WithMany("CreatedRoutes")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrandTripAPI.Models.RouteSeason", "Season")
                        .WithMany("Routes")
                        .HasForeignKey("SeasonId");

                    b.HasOne("GrandTripAPI.Models.RouteTheme", "Theme")
                        .WithMany("Routes")
                        .HasForeignKey("ThemeId");

                    b.Navigation("Creator");

                    b.Navigation("Season");

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("GrandTripAPI.Models.Route", b =>
                {
                    b.Navigation("Dots");

                    b.Navigation("Lines");
                });

            modelBuilder.Entity("GrandTripAPI.Models.RouteSeason", b =>
                {
                    b.Navigation("Routes");
                });

            modelBuilder.Entity("GrandTripAPI.Models.RouteTheme", b =>
                {
                    b.Navigation("Routes");
                });

            modelBuilder.Entity("GrandTripAPI.Models.User", b =>
                {
                    b.Navigation("CreatedRoutes");
                });
#pragma warning restore 612, 618
        }
    }
}
