using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductService.Migrations
{
    /// <inheritdoc />
    public partial class testproductadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ImageUrl", "Name", "Price" },
                values: new object[] { 1, "https://www.freepik.com/premium-ai-image/red-dead-redemption-2-sunset-silhouette_293762113.htm#query=red%20dead%20redemption%202&position=15&from_view=keyword&track=ais_hybrid&uuid=054b5cd5-a72c-4ab5-b2ef-9deebc54a008", "Test Product", 10m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
