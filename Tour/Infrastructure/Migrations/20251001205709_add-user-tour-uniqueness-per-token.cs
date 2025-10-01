using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addusertouruniquenesspertoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TourPurchaseTokens_TourId",
                schema: "tours",
                table: "TourPurchaseTokens");

            migrationBuilder.CreateIndex(
                name: "IX_TourPurchaseTokens_TourId_UserId",
                schema: "tours",
                table: "TourPurchaseTokens",
                columns: new[] { "TourId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TourPurchaseTokens_TourId_UserId",
                schema: "tours",
                table: "TourPurchaseTokens");

            migrationBuilder.CreateIndex(
                name: "IX_TourPurchaseTokens_TourId",
                schema: "tours",
                table: "TourPurchaseTokens",
                column: "TourId");
        }
    }
}
