using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTest1.Migrations
{
    public partial class PosIdInEmplRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionID",
                table: "Employees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionID",
                table: "Employees",
                nullable: false,
                defaultValue: 0);
        }
    }
}
