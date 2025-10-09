using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TourPurchaseTokenUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FollowerValidated",
                schema: "tours",
                table: "TourPurchaseTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IdentityValidated",
                schema: "tours",
                table: "TourPurchaseTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                schema: "tours",
                table: "TourPurchaseTokens",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "tours",
                table: "TourPurchaseTokens",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowerValidated",
                schema: "tours",
                table: "TourPurchaseTokens");

            migrationBuilder.DropColumn(
                name: "IdentityValidated",
                schema: "tours",
                table: "TourPurchaseTokens");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                schema: "tours",
                table: "TourPurchaseTokens");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "tours",
                table: "TourPurchaseTokens");
        }
    }
}
