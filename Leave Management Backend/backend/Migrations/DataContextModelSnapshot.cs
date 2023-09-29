﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend.Data;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("backend.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LeaveType")
                        .HasColumnType("int");

                    b.Property<int?>("ResolvementId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Request");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Comments = "This is for my hella sick vacay B)",
                            EndDate = new DateTime(2023, 9, 21, 11, 26, 40, 132, DateTimeKind.Local).AddTicks(6785),
                            LeaveType = 1,
                            ResolvementId = 1,
                            StartDate = new DateTime(2023, 9, 21, 11, 26, 40, 132, DateTimeKind.Local).AddTicks(6777),
                            UserId = 1
                        });
                });

            modelBuilder.Entity("backend.Models.Resolvement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdminId")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<int?>("RequestId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("RequestId")
                        .IsUnique();

                    b.ToTable("Resolvement");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AdminId = 2,
                            Comments = "Hell yeah B)",
                            IsApproved = true,
                            RequestId = 1
                        });
                });

            modelBuilder.Entity("backend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "123",
                            UserType = 0,
                            Username = "PersonName"
                        },
                        new
                        {
                            Id = 2,
                            Password = "123",
                            UserType = 1,
                            Username = "AdminName"
                        });
                });

            modelBuilder.Entity("backend.Models.Request", b =>
                {
                    b.HasOne("backend.Models.User", "User")
                        .WithMany("Requests")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Models.Resolvement", b =>
                {
                    b.HasOne("backend.Models.User", "Admin")
                        .WithMany("Resolvements")
                        .HasForeignKey("AdminId");

                    b.HasOne("backend.Models.Request", "Request")
                        .WithOne("Resolvement")
                        .HasForeignKey("backend.Models.Resolvement", "RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("backend.Models.Request", b =>
                {
                    b.Navigation("Resolvement");
                });

            modelBuilder.Entity("backend.Models.User", b =>
                {
                    b.Navigation("Requests");

                    b.Navigation("Resolvements");
                });
#pragma warning restore 612, 618
        }
    }
}
