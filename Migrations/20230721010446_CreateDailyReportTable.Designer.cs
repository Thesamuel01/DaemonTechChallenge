﻿// <auto-generated />
using System;
using DaemonTechChallenge.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DaemonTechChallenge.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230721010446_CreateDailyReportTable")]
    partial class CreateDailyReportTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DaemonTechChallenge.Models.DailyReport", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<decimal>("CaptcDia")
                        .HasColumnType("decimal(14, 2)");

                    b.Property<string>("CnpjFundo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("DtComptc")
                        .HasColumnType("Date");

                    b.Property<int>("NrCotst")
                        .HasColumnType("int");

                    b.Property<decimal>("ResgDia")
                        .HasColumnType("decimal(14, 2)");

                    b.Property<decimal>("VlPatrimLiq")
                        .HasColumnType("decimal(14, 2)");

                    b.Property<decimal>("VlQuota")
                        .HasColumnType("decimal(22, 12)");

                    b.Property<decimal>("VlTotal")
                        .HasColumnType("decimal(14, 2)");

                    b.HasKey("Id");

                    b.ToTable("DailyReport");
                });
#pragma warning restore 612, 618
        }
    }
}
