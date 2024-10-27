﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchedulingReportingMicroservice.Domain.Data;

#nullable disable

namespace SchedulingReportingService.Domain.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241026103327_editscheduletaskstable")]
    partial class editscheduletaskstable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SchedulingReportingService.Domain.Entities.Orders", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            OrderId = 1,
                            OrderDate = new DateTime(2024, 10, 26, 13, 33, 27, 516, DateTimeKind.Local).AddTicks(8890),
                            Status = "Placed",
                            UserId = 0
                        });
                });

            modelBuilder.Entity("SchedulingReportingService.Domain.Entities.ReportHistory", b =>
                {
                    b.Property<int>("ReportHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReportHistoryId"));

                    b.Property<DateTime>("GeneratedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("NewUsers")
                        .HasColumnType("int");

                    b.Property<string>("OrderIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalSales")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ReportHistoryId");

                    b.ToTable("ReportHistories");
                });

            modelBuilder.Entity("SchedulingReportingService.Domain.Entities.Sales", b =>
                {
                    b.Property<int>("SaleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SaleId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("SaleId");

                    b.HasIndex("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Sales");

                    b.HasData(
                        new
                        {
                            SaleId = 1,
                            Amount = 1000m,
                            Date = new DateTime(2024, 10, 26, 13, 33, 27, 516, DateTimeKind.Local).AddTicks(8875),
                            OrderId = 0,
                            UserId = 0
                        });
                });

            modelBuilder.Entity("SchedulingReportingService.Domain.Entities.ScheduledTasks", b =>
                {
                    b.Property<int>("ScheduledTaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduledTaskId"));

                    b.Property<string>("ApiKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CronExpression")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ScheduledTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TaskType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ScheduledTaskId");

                    b.ToTable("ScheduledTasks");
                });

            modelBuilder.Entity("SchedulingReportingService.Domain.Entities.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisteredDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Name = "Omar",
                            RegisteredDate = new DateTime(2024, 10, 26, 13, 33, 27, 516, DateTimeKind.Local).AddTicks(8699)
                        });
                });

            modelBuilder.Entity("SchedulingReportingService.Domain.Entities.Orders", b =>
                {
                    b.HasOne("SchedulingReportingService.Domain.Entities.Users", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SchedulingReportingService.Domain.Entities.Sales", b =>
                {
                    b.HasOne("SchedulingReportingService.Domain.Entities.Orders", "Order")
                        .WithMany("Sales")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchedulingReportingService.Domain.Entities.Users", "User")
                        .WithMany("Sales")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SchedulingReportingService.Domain.Entities.Orders", b =>
                {
                    b.Navigation("Sales");
                });

            modelBuilder.Entity("SchedulingReportingService.Domain.Entities.Users", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Sales");
                });
#pragma warning restore 612, 618
        }
    }
}
