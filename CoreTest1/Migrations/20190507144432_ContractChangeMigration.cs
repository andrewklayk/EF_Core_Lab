using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTest1.Migrations
{
    public partial class ContractChangeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Parts_PartID",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Parts_PartType_PartTypeID",
                table: "Parts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_PartID",
                table: "Contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartType",
                table: "PartType");

            migrationBuilder.DropColumn(
                name: "PartID",
                table: "Contracts");

            migrationBuilder.RenameTable(
                name: "PartType",
                newName: "PartTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartTypes",
                table: "PartTypes",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "ContractItems",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContractID = table.Column<int>(nullable: false),
                    PartID = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ContractItems_Contracts_ContractID",
                        column: x => x.ContractID,
                        principalTable: "Contracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractItems_Parts_PartID",
                        column: x => x.PartID,
                        principalTable: "Parts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CustomerID",
                table: "Contracts",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractItems_ContractID",
                table: "ContractItems",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractItems_PartID",
                table: "ContractItems",
                column: "PartID");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Customers_CustomerID",
                table: "Contracts",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_PartTypes_PartTypeID",
                table: "Parts",
                column: "PartTypeID",
                principalTable: "PartTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Customers_CustomerID",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Parts_PartTypes_PartTypeID",
                table: "Parts");

            migrationBuilder.DropTable(
                name: "ContractItems");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CustomerID",
                table: "Contracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartTypes",
                table: "PartTypes");

            migrationBuilder.RenameTable(
                name: "PartTypes",
                newName: "PartType");

            migrationBuilder.AddColumn<int>(
                name: "PartID",
                table: "Contracts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartType",
                table: "PartType",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PartID",
                table: "Contracts",
                column: "PartID");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Parts_PartID",
                table: "Contracts",
                column: "PartID",
                principalTable: "Parts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_PartType_PartTypeID",
                table: "Parts",
                column: "PartTypeID",
                principalTable: "PartType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
