﻿// <auto-generated />
using System;
using FitiChan.DL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FitiChan.DL.Migrations
{
    [DbContext(typeof(FitiDBContext))]
    [Migration("20231111170629_create")]
    partial class create
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FitiChan.DL.Entities.Message", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("DeliveryTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<ulong>("TargetChannelID")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("FitiChan.DL.Entities.User", b =>
                {
                    b.Property<ulong>("DSID")
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("AccountCreationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("AvataURL")
                        .HasColumnType("longtext");

                    b.Property<string>("AvatarID")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Globalname")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("DSID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
