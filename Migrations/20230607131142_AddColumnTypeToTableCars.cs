using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowRoomAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnTypeToTableCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Cars",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Cars");
        }
    }
}
