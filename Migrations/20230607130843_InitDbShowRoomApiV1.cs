using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowRoomAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitDbShowRoomApiV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    SerialNo = table.Column<string>(type: "text", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    ModelNo = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.SerialNo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
