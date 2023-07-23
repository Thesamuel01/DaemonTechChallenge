using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaemonTechChallenge.Migrations
{
    /// <inheritdoc />
    public partial class IncreaseDecimalRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "VlTotal",
                table: "DailyReport",
                type: "decimal(20,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "VlQuota",
                table: "DailyReport",
                type: "decimal(30,12)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(22,12)");

            migrationBuilder.AlterColumn<decimal>(
                name: "VlPatrimLiq",
                table: "DailyReport",
                type: "decimal(20,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "VlTotal",
                table: "DailyReport",
                type: "decimal(14,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "VlQuota",
                table: "DailyReport",
                type: "decimal(22,12)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(30,12)");

            migrationBuilder.AlterColumn<decimal>(
                name: "VlPatrimLiq",
                table: "DailyReport",
                type: "decimal(14,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)");
        }
    }
}
