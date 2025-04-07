using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniPyme.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SaleCode",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleCode",
                table: "Sales");
        }
    }
}
