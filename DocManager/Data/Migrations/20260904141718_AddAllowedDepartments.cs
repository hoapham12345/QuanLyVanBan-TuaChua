using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAllowedDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedDepartments",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedDepartments",
                table: "Documents");
        }
    }
}
