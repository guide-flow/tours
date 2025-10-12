using System;
using System.Collections.Generic;
using Core.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTourMetricsAndStatusChangeDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LengthInKm",
                schema: "tours",
                table: "Tours",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangeDate",
                schema: "tours",
                table: "Tours",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<ICollection<TransportDuration>>(
                name: "TransportDurations",
                schema: "tours",
                table: "Tours",
                type: "jsonb",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LengthInKm",
                schema: "tours",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "StatusChangeDate",
                schema: "tours",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "TransportDurations",
                schema: "tours",
                table: "Tours");
        }
    }
}
