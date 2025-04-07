using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniPyme.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSaleAndInvoiceForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Sales_SaleId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Clients_ClientId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ClientId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_SaleId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "Invoices");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_IdClient",
                table: "Sales",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_IdSale",
                table: "Invoices",
                column: "IdSale");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Sales_IdSale",
                table: "Invoices",
                column: "IdSale",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Clients_IdClient",
                table: "Sales",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Sales_IdSale",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Clients_IdClient",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_IdClient",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_IdSale",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ClientId",
                table: "Sales",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SaleId",
                table: "Invoices",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Sales_SaleId",
                table: "Invoices",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Clients_ClientId",
                table: "Sales",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
