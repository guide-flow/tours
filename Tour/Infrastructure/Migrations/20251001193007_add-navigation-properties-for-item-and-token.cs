using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addnavigationpropertiesforitemandtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TourPurchaseTokens_TourId",
                schema: "tours",
                table: "TourPurchaseTokens",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_TourId",
                schema: "tours",
                table: "ShoppingCartItems",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Tours_TourId",
                schema: "tours",
                table: "ShoppingCartItems",
                column: "TourId",
                principalSchema: "tours",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TourPurchaseTokens_Tours_TourId",
                schema: "tours",
                table: "TourPurchaseTokens",
                column: "TourId",
                principalSchema: "tours",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Tours_TourId",
                schema: "tours",
                table: "ShoppingCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TourPurchaseTokens_Tours_TourId",
                schema: "tours",
                table: "TourPurchaseTokens");

            migrationBuilder.DropIndex(
                name: "IX_TourPurchaseTokens_TourId",
                schema: "tours",
                table: "TourPurchaseTokens");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_TourId",
                schema: "tours",
                table: "ShoppingCartItems");
        }
    }
}
