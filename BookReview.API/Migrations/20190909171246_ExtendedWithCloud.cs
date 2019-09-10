using Microsoft.EntityFrameworkCore.Migrations;

namespace BookReview.API.Migrations
{
    public partial class ExtendedWithCloud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Pictures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Pictures");
        }
    }
}
