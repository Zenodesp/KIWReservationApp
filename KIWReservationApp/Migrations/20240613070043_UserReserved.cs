using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KIWReservationApp.Migrations
{
    /// <inheritdoc />
    public partial class UserReserved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserReserved",
                table: "Material",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserReserved",
                table: "Material");
        }
    }
}
