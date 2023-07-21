﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Loading_Related_Data.Migrations
{
    [DbContext(typeof(LoadingRelatedDataDbContext))]
    [Migration("20230721003026_mig_1")]
    partial class mig_1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.Property<int>("Salary")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Gençay",
                            RegionId = 1,
                            Salary = 1500,
                            Surname = "Yıldız"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Mahmut",
                            RegionId = 2,
                            Salary = 1500,
                            Surname = "Yıldız"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Rıfkı",
                            RegionId = 1,
                            Salary = 1500,
                            Surname = "Yıldız"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Cüneyt",
                            RegionId = 2,
                            Salary = 1500,
                            Surname = "Yıldız"
                        });
                });

            modelBuilder.Entity("Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EmployeeId = 1,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4799)
                        },
                        new
                        {
                            Id = 2,
                            EmployeeId = 1,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4819)
                        },
                        new
                        {
                            Id = 3,
                            EmployeeId = 2,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4823)
                        },
                        new
                        {
                            Id = 4,
                            EmployeeId = 2,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4826)
                        },
                        new
                        {
                            Id = 5,
                            EmployeeId = 3,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4829)
                        },
                        new
                        {
                            Id = 6,
                            EmployeeId = 3,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4832)
                        },
                        new
                        {
                            Id = 7,
                            EmployeeId = 3,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4835)
                        },
                        new
                        {
                            Id = 8,
                            EmployeeId = 4,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4837)
                        },
                        new
                        {
                            Id = 9,
                            EmployeeId = 4,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4838)
                        },
                        new
                        {
                            Id = 10,
                            EmployeeId = 1,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4839)
                        },
                        new
                        {
                            Id = 11,
                            EmployeeId = 2,
                            OrderDate = new DateTime(2023, 7, 21, 3, 30, 26, 581, DateTimeKind.Local).AddTicks(4841)
                        });
                });

            modelBuilder.Entity("Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Regions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Ankara"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Yozgat"
                        });
                });

            modelBuilder.Entity("Employee", b =>
                {
                    b.HasOne("Region", "Region")
                        .WithMany("Employees")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Order", b =>
                {
                    b.HasOne("Employee", "Employee")
                        .WithMany("Orders")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Employee", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Region", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
