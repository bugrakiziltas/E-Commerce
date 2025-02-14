using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Migrations
{
    /// <inheritdoc />
    public partial class orderbuyedproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyedProducts_Orders_OrderId",
                table: "BuyedProducts");

            migrationBuilder.DropIndex(
                name: "IX_BuyedProducts_OrderId",
                table: "BuyedProducts");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "BuyedProducts");

            migrationBuilder.DropColumn(
                name: "isDownloaded",
                table: "BuyedProducts");

            migrationBuilder.CreateTable(
                name: "OrderBuyedProducts",
                columns: table => new
                {
                    orderId = table.Column<int>(type: "integer", nullable: false),
                    buyedProductId = table.Column<int>(type: "integer", nullable: false),
                    isDownloaded = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBuyedProducts", x => new { x.orderId, x.buyedProductId });
                    table.ForeignKey(
                        name: "FK_OrderBuyedProducts_BuyedProducts_buyedProductId",
                        column: x => x.buyedProductId,
                        principalTable: "BuyedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderBuyedProducts_Orders_orderId",
                        column: x => x.orderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderBuyedProducts_buyedProductId",
                table: "OrderBuyedProducts",
                column: "buyedProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderBuyedProducts");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "BuyedProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isDownloaded",
                table: "BuyedProducts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_BuyedProducts_OrderId",
                table: "BuyedProducts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyedProducts_Orders_OrderId",
                table: "BuyedProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
