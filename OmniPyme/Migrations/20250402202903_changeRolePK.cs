using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OmniPyme.Web.Migrations
{
    /// <inheritdoc />
    public partial class changeRolePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdRol",
                table: "Roles",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "IdRol");
        }
    }
}
