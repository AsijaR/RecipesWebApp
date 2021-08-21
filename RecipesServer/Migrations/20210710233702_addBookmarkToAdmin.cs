using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipesServer.Migrations
{
    public partial class addBookmarkToAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bookmarks",
                columns: new[] { "BookmarkId", "UserId" },
                values: new object[] { 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookmarks",
                keyColumn: "BookmarkId",
                keyValue: 1);
        }
    }
}
