using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniPyme.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoicesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Clients_ClientIdClient",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "ClientIdClient",
                table: "Sales",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_ClientIdClient",
                table: "Sales",
                newName: "IX_Sales_ClientId");

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: true),
                    IdSale = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SaleId",
                table: "Invoices",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Clients_ClientId",
                table: "Sales",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Clients_ClientId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Sales",
                newName: "ClientIdClient");

            migrationBuilder.RenameIndex(
                name: "IX_Sales_ClientId",
                table: "Sales",
                newName: "IX_Sales_ClientIdClient");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Clients_ClientIdClient",
                table: "Sales",
                column: "ClientIdClient",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
