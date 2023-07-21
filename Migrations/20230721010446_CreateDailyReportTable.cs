using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaemonTechChallenge.Migrations
{
    /// <inheritdoc />
    public partial class CreateDailyReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DailyReport",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CnpjFundo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DtComptc = table.Column<DateTime>(type: "Date", nullable: false),
                    VlTotal = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    VlQuota = table.Column<decimal>(type: "decimal(22,12)", nullable: false),
                    VlPatrimLiq = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    CaptcDia = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    ResgDia = table.Column<decimal>(type: "decimal(14,2)", nullable: false),
                    NrCotst = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyReport", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyReport");
        }
    }
}
