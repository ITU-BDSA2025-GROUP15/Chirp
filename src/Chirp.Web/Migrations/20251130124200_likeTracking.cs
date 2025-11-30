using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Web.Migrations
{
    /// <inheritdoc />
    public partial class likeTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostOpinions",
                columns: table => new
                {
                    CheepId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostOpinions", x => new { x.CheepId, x.AuthorId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostOpinions");
        }
    }
}
