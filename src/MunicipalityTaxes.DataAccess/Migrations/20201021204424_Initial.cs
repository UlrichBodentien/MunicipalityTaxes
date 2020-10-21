using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MunicipalityTaxes.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Municipality",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipality", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityTaxType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityTaxType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityTax",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Tax = table.Column<double>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    TaxTypeId = table.Column<int>(nullable: false),
                    MunicipalityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityTax", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalityTax_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MunicipalityTax_MunicipalityTaxType_TaxTypeId",
                        column: x => x.TaxTypeId,
                        principalTable: "MunicipalityTaxType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MunicipalityTaxType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Daily" },
                    { 1, "Weekly" },
                    { 2, "Monthly" },
                    { 3, "Yearly" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityTax_MunicipalityId",
                table: "MunicipalityTax",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityTax_TaxTypeId",
                table: "MunicipalityTax",
                column: "TaxTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MunicipalityTax");

            migrationBuilder.DropTable(
                name: "Municipality");

            migrationBuilder.DropTable(
                name: "MunicipalityTaxType");
        }
    }
}
