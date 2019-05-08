using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTest1.Migrations
{
    public partial class ModifyStockEmplScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Stocks_StockID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_StockID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "StockID",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "PositionID",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmployeeID = table.Column<int>(nullable: false),
                    StockID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Positions_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Positions_Stocks_StockID",
                        column: x => x.StockID,
                        principalTable: "Stocks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Positions_EmployeeID",
                table: "Positions",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_StockID",
                table: "Positions",
                column: "StockID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropColumn(
                name: "PositionID",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "StockID",
                table: "Employees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StockID",
                table: "Employees",
                column: "StockID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Stocks_StockID",
                table: "Employees",
                column: "StockID",
                principalTable: "Stocks",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
