﻿// <auto-generated />
using System;
using Carpool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Carpool.Infrastructure.Migrations
{
    [DbContext(typeof(CarpoolDbContext))]
    [Migration("20240103112440_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Carpool.Domain.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("InsuranceExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfSeats")
                        .HasColumnType("integer");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TechnicalInspectionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<int?>("ReservationId")
                        .HasColumnType("integer");

                    b.Property<int?>("TripId")
                        .HasColumnType("integer");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ReservationId");

                    b.HasIndex("TripId");

                    b.HasIndex("UserId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ReservedSeats")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TripId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TripId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TokenString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AvailableSeats")
                        .HasColumnType("integer");

                    b.Property<int>("CarId")
                        .HasColumnType("integer");

                    b.Property<int?>("DepartureAddressId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("DestinationAddressId")
                        .HasColumnType("integer");

                    b.Property<int>("DriverId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsSmokingAllowed")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("DepartureAddressId");

                    b.HasIndex("DestinationAddressId");

                    b.HasIndex("DriverId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Address", b =>
                {
                    b.HasOne("Carpool.Domain.Entities.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Car", b =>
                {
                    b.HasOne("Carpool.Domain.Entities.User", "Owner")
                        .WithMany("Cars")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Feedback", b =>
                {
                    b.HasOne("Carpool.Domain.Entities.Reservation", "Reservation")
                        .WithMany()
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Carpool.Domain.Entities.Trip", "Trip")
                        .WithMany("Feedbacks")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Carpool.Domain.Entities.User", "User")
                        .WithMany("FeedbacksGiven")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Reservation");

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Reservation", b =>
                {
                    b.HasOne("Carpool.Domain.Entities.Trip", "Trip")
                        .WithMany("Reservations")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Carpool.Domain.Entities.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Token", b =>
                {
                    b.HasOne("Carpool.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Trip", b =>
                {
                    b.HasOne("Carpool.Domain.Entities.Car", "Car")
                        .WithMany("Trips")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Carpool.Domain.Entities.Address", "DepartureAddress")
                        .WithMany()
                        .HasForeignKey("DepartureAddressId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Carpool.Domain.Entities.Address", "DestinationAddress")
                        .WithMany()
                        .HasForeignKey("DestinationAddressId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Carpool.Domain.Entities.User", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("DepartureAddress");

                    b.Navigation("DestinationAddress");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Car", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.Trip", b =>
                {
                    b.Navigation("Feedbacks");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Carpool.Domain.Entities.User", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Cars");

                    b.Navigation("FeedbacksGiven");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
