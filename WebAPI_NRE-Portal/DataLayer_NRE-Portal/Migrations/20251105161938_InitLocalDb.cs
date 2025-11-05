using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer_NRE_Portal.Migrations
{
    /// <inheritdoc />
    public partial class InitLocalDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Installations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    EnergyType = table.Column<string>(type: "TEXT", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    InstalledCapacityKW = table.Column<double>(type: "REAL", nullable: false),
                    AnnualProductionKWh = table.Column<double>(type: "REAL", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    SourceX = table.Column<double>(type: "REAL", nullable: true),
                    SourceY = table.Column<double>(type: "REAL", nullable: true),
                    SourceCrs = table.Column<string>(type: "TEXT", nullable: true),
                    CommissioningDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Installations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    EnergyType = table.Column<string>(type: "TEXT", nullable: false),
                    ProductionKWh = table.Column<double>(type: "REAL", nullable: false),
                    Canton = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionSummaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrivateInstallations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IntegrationType = table.Column<string>(type: "TEXT", nullable: true),
                    PvCellType = table.Column<string>(type: "TEXT", nullable: true),
                    Azimuth = table.Column<int>(type: "INTEGER", nullable: true),
                    RoofSlope = table.Column<int>(type: "INTEGER", nullable: true),
                    LengthM = table.Column<double>(type: "REAL", nullable: true),
                    WidthM = table.Column<double>(type: "REAL", nullable: true),
                    AreaM2 = table.Column<double>(type: "REAL", nullable: true),
                    EstimatedKWh = table.Column<double>(type: "REAL", nullable: true),
                    LocationText = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateInstallations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateInstallations_Installations_Id",
                        column: x => x.Id,
                        principalTable: "Installations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicInstallations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OperatorName = table.Column<string>(type: "TEXT", nullable: true),
                    Municipality = table.Column<string>(type: "TEXT", nullable: true),
                    SourceRef = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicInstallations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicInstallations_Installations_Id",
                        column: x => x.Id,
                        principalTable: "Installations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Installations_Latitude_Longitude",
                table: "Installations",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_Installations_Region_EnergyType",
                table: "Installations",
                columns: new[] { "Region", "EnergyType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateInstallations");

            migrationBuilder.DropTable(
                name: "ProductionSummaries");

            migrationBuilder.DropTable(
                name: "PublicInstallations");

            migrationBuilder.DropTable(
                name: "Installations");
        }
    }
}
