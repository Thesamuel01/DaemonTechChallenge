using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaemonTechChallenge.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableEngineToMyISAM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE DailyReport ENGINE = MyISAM;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE DailyReport ENGINE = InnoDB;");
        }
    }
}
