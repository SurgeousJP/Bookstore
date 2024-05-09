using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookCatalog.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBookReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "book_reviews",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<long>(type: "bigint", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    user_profile_image = table.Column<string>(type: "text", nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true),
                    rating_point = table.Column<decimal>(type: "numeric", nullable: true),
                    creation_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("book_reviews_pkey", x => new { x.user_id, x.book_id });
                    table.ForeignKey(
                        name: "book_reviews_book_id_fkey",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id");
                });
            migrationBuilder.CreateIndex(
                name: "IX_book_reviews_book_id",
                table: "book_reviews",
                column: "book_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
