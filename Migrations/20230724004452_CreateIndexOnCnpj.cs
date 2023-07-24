using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaemonTechChallenge.Migrations
{
    /// <inheritdoc />
    public partial class CreateIndexOnCnpj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DailyReport_Cnpj",
                table: "DailyReport",
                column: "CnpjFundo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NomeDaTabela_Cnpj",
                table: "DailyReport");
        }
    }
}
