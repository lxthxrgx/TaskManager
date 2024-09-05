﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TaskManager.Database;

#nullable disable

namespace TaskManager.Migrations
{
    [DbContext(typeof(TakClassDatabase))]
    [Migration("20240905184025_Initial-Create")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TaskManager.Class.EmploymentClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("EmploymentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("employments");
                });

            modelBuilder.Entity("TaskManager.Class.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("EmpName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("EmploymentId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EmploymentId");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("TaskManager.Class.TaskClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("EmploymentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("StatusId")
                        .HasColumnType("integer");

                    b.Property<string>("Task")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EmploymentId");

                    b.HasIndex("StatusId");

                    b.ToTable("TaskClasses");
                });

            modelBuilder.Entity("TaskManager.Class.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskManager.Class.UserEmployment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EmploymentId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EmploymentId");

                    b.HasIndex("UserId", "EmploymentId")
                        .IsUnique();

                    b.ToTable("UserEmployments");
                });

            modelBuilder.Entity("TaskManager.Class.Status", b =>
                {
                    b.HasOne("TaskManager.Class.EmploymentClass", "Employment")
                        .WithMany()
                        .HasForeignKey("EmploymentId");

                    b.Navigation("Employment");
                });

            modelBuilder.Entity("TaskManager.Class.TaskClass", b =>
                {
                    b.HasOne("TaskManager.Class.EmploymentClass", "Employment")
                        .WithMany("TaskClasses")
                        .HasForeignKey("EmploymentId");

                    b.HasOne("TaskManager.Class.Status", null)
                        .WithMany("TaskClasses")
                        .HasForeignKey("StatusId");

                    b.Navigation("Employment");
                });

            modelBuilder.Entity("TaskManager.Class.UserEmployment", b =>
                {
                    b.HasOne("TaskManager.Class.EmploymentClass", "Employment")
                        .WithMany("UserEmployments")
                        .HasForeignKey("EmploymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Class.User", "User")
                        .WithMany("UserEmployments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskManager.Class.EmploymentClass", b =>
                {
                    b.Navigation("TaskClasses");

                    b.Navigation("UserEmployments");
                });

            modelBuilder.Entity("TaskManager.Class.Status", b =>
                {
                    b.Navigation("TaskClasses");
                });

            modelBuilder.Entity("TaskManager.Class.User", b =>
                {
                    b.Navigation("UserEmployments");
                });
#pragma warning restore 612, 618
        }
    }
}
