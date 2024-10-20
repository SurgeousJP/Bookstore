using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookCatalog.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGenreImageURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "genres",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "genres");
        }
    }
}
