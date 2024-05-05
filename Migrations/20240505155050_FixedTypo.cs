using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork.Migrations
{
    /// <inheritdoc />
    public partial class FixedTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActice",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "IsActice",
                table: "Admin",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "IsActice");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Admin",
                newName: "IsActice");
        }
    }
}
