using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniPyme.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SaleDetailProductCode",
                table: "SaleDetails",
                column: "SaleDetailProductCode");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetails_Products_SaleDetailProductCode",
                table: "SaleDetails",
                column: "SaleDetailProductCode",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetails_Products_SaleDetailProductCode",
                table: "SaleDetails");

            migrationBuilder.DropIndex(
                name: "IX_SaleDetails_SaleDetailProductCode",
                table: "SaleDetails");
        }
    }
}
