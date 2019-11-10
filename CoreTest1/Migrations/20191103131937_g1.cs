using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTest1.Migrations
{
    public partial class g1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "longt",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "lat",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "longt",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "lat",
                table: "Stocks",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
