using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTierAndBranchStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tier",
                table: "Customers");

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "WashBays",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TierId",
                table: "Customers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BayId",
                table: "Bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "Bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WashBayId",
                table: "Bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Hotline = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    OperatingHours = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TierName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PointRate = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BookingWindow = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tiers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tiers",
                columns: new[] { "Id", "BookingWindow", "Level", "PointRate", "TierName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 7, 4, 1.00m, "Bronze" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 10, 3, 1.20m, "Silver" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 12, 2, 1.50m, "Gold" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 14, 1, 2.00m, "Diamond" }
                });

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000001"),
                column: "BranchId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000002"),
                column: "BranchId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "WashBays",
                keyColumn: "Id",
                keyValue: new Guid("b1b2c3d4-0001-0001-0001-000000000003"),
                column: "BranchId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WashBays_BranchId",
                table: "WashBays",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TierId",
                table: "Customers",
                column: "TierId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BranchId",
                table: "Bookings",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_WashBayId",
                table: "Bookings",
                column: "WashBayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_WashBays_WashBayId",
                table: "Bookings",
                column: "WashBayId",
                principalTable: "WashBays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Tiers_TierId",
                table: "Customers",
                column: "TierId",
                principalTable: "Tiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WashBays_Branches_BranchId",
                table: "WashBays",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_WashBays_WashBayId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Tiers_TierId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_WashBays_Branches_BranchId",
                table: "WashBays");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "Tiers");

            migrationBuilder.DropIndex(
                name: "IX_WashBays_BranchId",
                table: "WashBays");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TierId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BranchId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_WashBayId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "WashBays");

            migrationBuilder.DropColumn(
                name: "TierId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "BayId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "WashBayId",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "Tier",
                table: "Customers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
