﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OmniPyme.Data;

#nullable disable

namespace OmniPyme.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250402193210_AddInvoicesTable")]
    partial class AddInvoicesTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OmniPyme.Web.Data.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DNI")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("LastPurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DNI")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("OmniPyme.Web.Data.Entities.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("IdSale")
                        .HasColumnType("int");

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InvoiceNumber")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<int?>("SaleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SaleId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("OmniPyme.Web.Data.Entities.Role", b =>
                {
                    b.Property<int>("IdRol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRol"));

                    b.Property<string>("RolName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdRol");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("OmniPyme.Web.Data.Entities.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SalePaymentMethod")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<decimal>("SaleTotal")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("OmniPyme.Web.Data.Entities.Invoice", b =>
                {
                    b.HasOne("OmniPyme.Web.Data.Entities.Sale", "Sale")
                        .WithMany()
                        .HasForeignKey("SaleId");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("OmniPyme.Web.Data.Entities.Sale", b =>
                {
                    b.HasOne("OmniPyme.Web.Data.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.Navigation("Client");
                });
#pragma warning restore 612, 618
        }
    }
}
